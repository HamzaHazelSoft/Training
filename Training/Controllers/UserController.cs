using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        //Return all users exist in DB

        [HttpGet]
        public IActionResult GetAll()
        {
            var Users = _userService.GetAllUsers();
            return Ok(Users); 
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
            return Ok($"User  {(status ? " added successfully" : " could not be added")}");
        }


        // Returns a user by its Id
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            User User = _userService.GetUserById(id);
            return User != null ? Ok(User) : Ok(new { message = "User not found" });
        }

        // Deletes a user by Id

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            bool status = _userService.DeleteUserById(id);
            return status == true ? Ok(new { message = "User deleted successfully" }) :
                Ok(new { message = "Failed to delete user" });
        }

        // Updates an existing user

        [HttpPut("{id}")]
        public IActionResult Update(string id, UserDTO User)
        {
            bool status = _userService.UpdateUserById(id,User);
            return status == true ? Ok(new { message = "User updated successfully" }) : 
                Ok(new { message = "Failed to update user" });
        }
    }
}
