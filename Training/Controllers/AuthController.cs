using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Training.DTOs;
using Training.Models;
using Training.Services.Auth;
using Training.Services.Jwt;

namespace Training.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {

        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {
                var response = await _authService.Register(registerDTO);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest("An error occurred while registering user", ex);
            }
        }

        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword(SetPasswordDTO passwordDTO)
        {
            try
            {
                var response = await _authService.SetPassword(passwordDTO);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while settting password", ex);
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                var response = await _authService.ConfirmEmail(userId, token);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while confirming email", ex);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                var response = await _authService.Login(loginDTO);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while logging in user", ex);
            }
        }




    }
}
