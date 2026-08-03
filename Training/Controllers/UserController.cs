using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection.Metadata.Ecma335;
using Training.DTOs;
using Training.Models;
using Training.Services;

namespace Training.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService UserService)
        {
            _userService = UserService;
        }

        // Adds a new user into the database
        [HttpPost]
        public IActionResult Create([FromBody]User user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool status = _userService.AddUser(user);
            return status
                ? Ok( new { message = "User created successfully" })
                : BadRequest(new { message = "Failed to create user" }); //If invalid data is sent
        }


        // Returns a user by its Id
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            User User = _userService.GetUserById(id);
            return User != null ? Ok(User) : NotFound(new { message = "User not found" });
        }

        // Deletes a user by Id

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            bool status = _userService.DeleteUserById(id);
            return status == true ? Ok(new { message = "User deleted successfully" }) :
                StatusCode(500, new { message = "Failed to delete user" }); //Internal Server Error
        }

        // Updates an existing user

        [HttpPut("{id}")]
        public IActionResult Update(string id, UserDTO User)
        {
            bool status = _userService.UpdateUserById(id,User);
            return status == true ? Ok(new { message = "User updated successfully" }) : 
                StatusCode(500, new { message = "Failed to update user" }); //Internal Server Error
        }
    }
}
