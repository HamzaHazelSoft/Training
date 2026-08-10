using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;
using UserManagementSystem.Services.Jwt;
using UserManagementSystem.Services.Mail;
using Hangfire;
using System.Transactions;
using AutoMapper;
using static UserManagementSystem.Helper.Constant;
using Microsoft.AspNetCore.Http.Features;

namespace UserManagementSystem.Services.Auth.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager; //For registering
        private readonly SignInManager<User> _signinManager; //For login
        private readonly IJwtService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        public AuthService(UserManager<User> userManager, SignInManager<User> signinManager,
            IJwtService tokenService,IConfiguration configuration,IMailService service,IMapper mapper)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _mailService = service;
            _mapper = mapper;
        }

        public async Task<RegisterResponseDTO<UserDTO>> Register(RegisterDTO registerDTO)
        {
            User user;

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
                    foreach(var error in result.Errors)
                    {
                        if(error.Code == nameof(IdentityErrorDescriber.DuplicateEmail))
                        {
                            throw new InvalidOperationException(MessageConstants.UserAlreadyExists);
                        }
                        else if(error.Code == nameof(IdentityErrorDescriber.DuplicateUserName))
                        {
                            throw new InvalidOperationException(MessageConstants.UsernameAlreadyTaken);
                        }
                    }
                }

                foreach (var role in registerDTO.Roles)
                {
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(user, role);

                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            throw new InvalidOperationException(error.Description);
                        }
                    }
                }

                scope.Complete();

            }

            _ = _mailService.SendConfirmationEmailAsync(user);

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            return new RegisterResponseDTO<UserDTO>
            {
                Entity = _mapper.Map<UserDTO>(user),
                PasswordResetToken = passwordResetToken
            };

        }

        public async Task<UserDTO> ConfirmEmail(string userId,string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new InvalidOperationException(MessageConstants.InvalidConfirmationLink);

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                throw new InvalidOperationException(MessageConstants.InvalidConfirmationLink);

            return _mapper.Map<UserDTO>(user);

        }

        public async Task<UserDTO> SetPassword(SetPasswordDTO passwordDTO)
        {
            var user = await _userManager.FindByIdAsync(passwordDTO.UserId);

            if (user == null)
                throw new Exception(MessageConstants.UserNotFound);

            var result = await _userManager.ResetPasswordAsync(
                user,
                passwordDTO.Token,
                passwordDTO.NewPassword
            );

            if (!result.Succeeded)
                throw new InvalidOperationException(MessageConstants.FailedToSetPassword);

            return _mapper.Map<UserDTO>(user);
           
        }
        public async Task<string> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
                throw new UnauthorizedAccessException(MessageConstants.UserNotFound);

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException(MessageConstants.InvalidUsernameOrPassword);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            return token;
        }
    }
}
