using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Training.DTOs;
using Training.Helper;
using Training.Models;
using Training.Services.Jwt;

namespace Training.Services.Auth.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager; //For registering
        private readonly SignInManager<User> _signinManager; //For login
        private readonly ITokenService _tokenService;
        public AuthService(UserManager<User> userManager, SignInManager<User> signinManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _tokenService = tokenService;
        }
        public async Task<Response<User>> Register(RegisterDTO registerDTO)
        {
            var user = new User
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
            };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
                return Response<User>.FailureResponse("Failed to register user");

            await _userManager.AddToRoleAsync(user, "User");

            return Response<User>.SuccessResponse("User registered Successfully",user);
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
