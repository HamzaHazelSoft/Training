using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;
using Training.DTOs;
using Training.Helper;
using Training.Models;
using Training.Services;

namespace Training.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        /* DI Because it losely couples the application making it easier to maintain. In simple words, we achieved the
        Inversion of Control principle by using dependency injection. The service does not create the context and mapper instances,
        but rather receives them from the outside means Framework
*/
        public UserController(IUserService UserService) 
        {
            _userService = UserService;
        }

        // Create User
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                bool status = await _userService.AddUser(user);

                if (status)
                    return Ok("User created successfully", user); //Method1

                return BadRequest("User creation failed");
            }
            catch(Exception ex)
            {
                return BadRequest("An error occurred while creating user", ex);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _userService.GetUserById(id);

                if (user != null)
                    return Ok("User retrieved successfully", user); //Method1

                return BadRequest("User not found");
            }
            catch(Exception ex)
            {
                return BadRequest("An error occurred while retrieving user", ex);
            }
        }

        // Delete User
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                bool status = await _userService.DeleteUserById(id);

                if (status)
                    return Ok("User deleted successfully");

                return BadRequest("User not found");
            }
            catch(Exception ex)
            {
                return BadRequest("An error occurred while deleting user", ex);
            }
        }

        // Update User
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UserDTO user)
        {
            try
            {
                bool status = await _userService.UpdateUserById(id, user);

                if (status)
                    return Ok("User updated successfully", user); //Method1

                return BadRequest("User not found");
            }
            catch(Exception ex)
            {
                return BadRequest("An error occurred while updating user", ex);
            }
        }
    }
}
