using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;
using UserManagementSystem.Services.Auth;
using UserManagementSystem.Services.Jwt;
using static UserManagementSystem.Helper.Constant;

namespace UserManagementSystem.Controllers
{
    public class AuthController : BaseController
    {

        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService,ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {
                var response = await _authService.Register(registerDTO);
                return Ok(MessageConstants.UserCreatedSuccessfully, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration request failed. Email: {Email}", registerDTO.Email);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword(SetPasswordDTO passwordDTO)
        {
            try
            {
                var response = await _authService.SetPassword(passwordDTO);
                return Ok(MessageConstants.PasswordSetSuccessfully,response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Set-password request failed. UserId: {UserId}", passwordDTO.UserId);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                var UserDTO = await _authService.ConfirmEmail(userId, token);
                return Ok(MessageConstants.EmailConfirmedSuccessfully, UserDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Email confirmation failed. UserId: {UserId}", userId);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                LoginResponseDTO loginResponse = await _authService.Login(loginDTO);
                return Ok(MessageConstants.LoginSuccessful,loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Login request denied. Email: {Email}",loginDTO.Email);
                return BadRequest(ex.Message);
            }
        }

    }
}
