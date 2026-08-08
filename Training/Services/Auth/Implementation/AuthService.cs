using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Training.DTOs;
using Training.Helper;
using static Training.Helper.EmailTemplate;
using Training.Models;
using Training.Services.Jwt;
using Training.Services.Mail;
using Hangfire;

namespace Training.Services.Auth.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager; //For registering
        private readonly SignInManager<User> _signinManager; //For login
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        public AuthService(UserManager<User> userManager, SignInManager<User> signinManager,
            ITokenService tokenService,IConfiguration configuration,IMailService service)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _mailService = service;
        }
        public async Task<Response<User>> Register(RegisterDTO registerDTO)
        {
            var user = new User
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
            };
            var result = await _userManager.CreateAsync(user,"1122");

            if (!result.Succeeded)
                return Response<User>.FailureResponse("Failed to register user");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var baseURL = _configuration["AppSettings:BaseUrl"];

            var confirmationLink =$"{baseURL}/api/Auth/confirm-email?userId={user.Id}&token={encodedToken}";

            var subject = GetConfirmationEmailSubject();

            var body = GetConfirmationEmailBody(confirmationLink);

            BackgroundJob.Enqueue<IMailService>(
                service => service.SendMailAsync(
                    user.Email!,
                    subject,
                    body
                )
            );

            await _userManager.AddToRoleAsync(user, "User");

            var newResettoken = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Response<User>.SuccessResponse($"User registered Successfully {newResettoken}",user);
        }

        public async Task<Response<User>> ConfirmEmail(string userId,string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Response<User>.FailureResponse("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return Response<User>.FailureResponse(
                    "Invalid or expired confirmation token");  

            return Response<User>.SuccessResponse(
                "Email confirmed successfully",
                user);
        }

        public async Task<Response<User>> SetPassword(SetPasswordDTO passwordDTO)
        {
            var user = await _userManager.FindByIdAsync(passwordDTO.UserId);

            if (user == null)
                return Response<User>.FailureResponse("User not found");

            var result = await _userManager.ResetPasswordAsync(
                user,
                passwordDTO.Token,
                passwordDTO.NewPassword
            );

            if (!result.Succeeded)
                return Response<User>.FailureResponse("Failed to set password");

            return Response<User>.SuccessResponse(
                "Password set successfully",
                user
            );
        }
        public async Task<Response<string>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
                return Response<string>.FailureResponse("User not found");

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded)
                return Response<string>.FailureResponse("Invalid password");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            return Response<string>.SuccessResponse("Login successful", token);
        }
    }
}
