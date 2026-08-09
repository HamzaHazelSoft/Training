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
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {
                var response = await _authService.Register(registerDTO);
                return Ok(MessageConstants.UserCreatedSuccessfully, response);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(MessageConstants.ErrorRegisteringUser, ex);
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
            catch(KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(MessageConstants.ErrorSettingPassword, ex);
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(MessageConstants.ErrorConfirmingEmail, ex);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                string token = await _authService.Login(loginDTO);
                return Ok(MessageConstants.LoginSuccessful,token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(MessageConstants.ErrorLoggingIn, ex);
            }
        }

    }
}
