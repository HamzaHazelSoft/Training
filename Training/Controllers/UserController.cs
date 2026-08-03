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

        // Adds a new user into the database
        [HttpPost]
        public IActionResult Create(User user)
        {
            try
            {
                bool status = _userService.AddUser(user);

                if (status)
                {
                    return Object<User>(user, "User created successfully");
                }

                return Object("User creation failed");
            }
            catch (Exception ex)
            {
                return Object(
                    ex, 
                    "An error occurred while creating user"
                );
            }
        }


        // Returns a user by its Id and id recieve as Route Param
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                if (user != null)
                {
                    return Object<User>(user, "User retrieved successfully");
                }
                return Object("User not found");
            }
            catch (Exception ex)
            {
                return Object(
                    ex,
                    "An error occurred while retrieving user"   
                );
            }

        }

        // Deletes a user by Id

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                bool status = _userService.DeleteUserById(id);
                if (status)
                {
                    return Response<object>(null, "User deleted successfully");
                }
                return Response("User not found");
            }
            catch (Exception ex)
            {
                return Response(
                    ex,
                    "An error occurred while deleting user"
                );
            }
        }

        // Updates an existing user

        [HttpPut("{id}")]
        public IActionResult Update(string id, UserDTO User)
        {
            try
            {
                bool status = _userService.UpdateUserById(id, User);
                if (status)
                {
                    return Response<UserDTO>(User, "User updated successfully");
                }
                return Response("User not found");

            }
            catch (Exception ex)
            {
                return Response(
                    ex,
                    "An error occurred while updating user"
                );
            }
        }
    }
}
