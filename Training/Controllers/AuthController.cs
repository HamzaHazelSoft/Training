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
                var result = await _authService.Register(registerDTO);
                if (!result)
                {
                    return BadRequest("Failed to register user");
                }
                return Ok("User registered successfully");
            }
            catch(Exception ex)
            {
                return BadRequest("An error occurred while registering user", ex);
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
