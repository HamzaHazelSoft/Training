using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using Serilog.Core;
using System.Transactions;
using UserManagementSystem.DTOs;
using UserManagementSystem.Helper;
using UserManagementSystem.Models;
using UserManagementSystem.Services.Jwt;
using UserManagementSystem.Services.Mail;
using static UserManagementSystem.Helper.Constant;

namespace UserManagementSystem.Services.Auth.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager; //For registering
        private readonly SignInManager<User> _signinManager; //For login
        private readonly IJwtService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        public AuthService(UserManager<User> userManager, SignInManager<User> signinManager,
            IJwtService tokenService,IConfiguration configuration,
            IMailService service,IMapper mapper,
            RoleManager<IdentityRole> roleManager,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _mailService = service;
            _mapper = mapper;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<UserDTO> Register(RegisterDTO registerDTO)
        {
            using var userNameContext = LogContext.PushProperty("UserName", registerDTO.UserName);

            _logger.LogInformation("User registration started. Email: {Email}", registerDTO.Email);

            User user;

            var existingUser = await _userManager.FindByNameAsync(registerDTO.UserName);

            if (existingUser != null)
            {
                throw new Exception(MessageConstants.UsernameAlreadyTaken);
            }

            var existingEmail = await _userManager.FindByEmailAsync(registerDTO.Email);

            if (existingEmail != null)
            {
                throw new Exception(MessageConstants.UserAlreadyExists);
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                 user = new User
                {
                    UserName = registerDTO.UserName,
                    Email = registerDTO.Email,
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    DOB = registerDTO.DOB.Value,
                    PhoneNumber = registerDTO.PhoneNumber
                };

                IdentityResult result = await _userManager.CreateAsync(user);

                if (!result.Succeeded) 
                {
                    _logger.LogWarning("User creation failed. Errors: {Errors}",string.Join(" | ", result.Errors.Select(e => e.Description)));
                    throw new Exception(MessageConstants.UserCreationFailed);
                }

                foreach (var roleId in registerDTO.Roles)
                {
                    var role = await _roleManager.FindByIdAsync(roleId);

                    if (role == null)
                    {
                        _logger.LogWarning("Invalid role entered. RoleId: {RoleId}", roleId);
                        throw new Exception(MessageConstants.InvalidRolesEntered);
                    }

                    IdentityResult roleResult = await _userManager.AddToRoleAsync(user, role.Name!);

                    if (!roleResult.Succeeded)
                    {
                        _logger.LogWarning("Role assignment failed. RoleId: {RoleId}, Error: {Error}",
                            roleId,string.Join("|", roleResult.Errors.Select(e=>e.Description)));

                        foreach (var error in roleResult.Errors)
                        {
                            throw new Exception(error.Description);
                        }
                    }
                }

                scope.Complete();

            }

            _logger.LogInformation("User registered successfully. UserId: {UserId}", user.Id);

            _ = _mailService.SendConfirmationEmailAsync(user);
            return _mapper.Map<UserDTO>(user);

        }

        public async Task<RegisterResponseDTO> ConfirmEmail(string userId,string token)
        {
            using var _uid = LogContext.PushProperty("UserId", userId);

            _logger.LogInformation("Email confirmation started.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception(MessageConstants.UserNotFound);
            }

            using var userNameContext = LogContext.PushProperty("UserName", user.UserName);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded) {
                _logger.LogWarning("Email confirmation failed - invalid/expired token. Errors: {Errors}",
                    string.Join(" | ", result.Errors.Select(e => e.Description)));
                throw new Exception(MessageConstants.InvalidConfirmationLink);
            }


            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            _logger.LogInformation("Email confirmed successfully.");

            return new RegisterResponseDTO
            {
                UserDTO = _mapper.Map<UserDTO>(user),
                PasswordResetToken = passwordResetToken
            };

        }

        public async Task<UserDTO> SetPassword(SetPasswordDTO passwordDTO)
        {

            using var _uid = LogContext.PushProperty("UserId", passwordDTO.UserId);
            _logger.LogInformation("Password setup started.");

            // 1. Validate password format first
            var passwordValidation = await _userManager.PasswordValidators.
                First().ValidateAsync(
                    _userManager,
                    null!,
                    passwordDTO.NewPassword
                );

            if (!passwordValidation.Succeeded) {
                _logger.LogWarning("Password setup failed - password validation failed. {NewPassword} entered: ",passwordDTO.NewPassword);
                throw new Exception(Constant.FailedToSetPassword(_configuration));
            } 

            // 2. Validate user
            var user = await _userManager.FindByIdAsync(passwordDTO.UserId);

            if (user == null) {
                _logger.LogWarning("Password setup failed ");
                throw new Exception(MessageConstants.InvalidUserId);
            }

            using var userNameContext = LogContext.PushProperty("UserName", user.UserName);
            // 3. Validate token and reset password
            var result = await _userManager.ResetPasswordAsync(
                user,
                passwordDTO.PasswordResetToken,
                passwordDTO.NewPassword
            );

            if (!result.Succeeded) {
                _logger.LogError("Password setup failed - invalid/expired reset token.");
                throw new Exception(MessageConstants.InvalidPasswordResetToken);
            }

            _logger.LogInformation("Password set successfully.");

            return _mapper.Map<UserDTO>(user);
        }
        public async Task<LoginResponseDTO> Login(LoginDTO loginDTO)
        {
            _logger.LogInformation("Login attempt started. Email: {Email}", loginDTO.Email);

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null) throw new UnauthorizedAccessException(MessageConstants.InvalidUsernameOrPassword);
            using var userNameContext = LogContext.PushProperty("UserName", user.UserName);
            if (!user.EmailConfirmed) throw new UnauthorizedAccessException(MessageConstants.EmailNotConfirmed);

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException(MessageConstants.InvalidUsernameOrPassword);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            _logger.LogInformation("Login successful. UserId: {UserId}", user.Id);
            return new LoginResponseDTO
            {
                UserDTO = _mapper.Map<UserDTO>(user),
                JwtToken = token
            };
        }
    }
}
