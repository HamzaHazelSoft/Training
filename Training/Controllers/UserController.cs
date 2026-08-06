using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
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

        //GetUser
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest paginationRequest)
        {
            try {
                var response = await _userService.GetUsers(paginationRequest);
                return Ok(Constant.MessageConstants.UserRetrievedSuccessfully, response);
            }
            catch(Exception ex)
            {
                return BadRequest(Constant.MessageConstants.InvalidSorting,ex);
            }
        }   

        // Create User
        [HttpPost]
        public async Task<IActionResult> Create(UserDTO user)
        {
            try
            {
                bool status = await _userService.AddUser(user);

                if (status)
                    return Ok(Constant.MessageConstants.UserCreatedSuccessfully, user); //Method1

                return BadRequest(Constant.MessageConstants.UserCreationFailed);
            }
            catch(Exception ex)
            {
                return BadRequest(Constant.MessageConstants.ErrorCreatingUser, ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _userService.GetUserById(id);

                if (user != null)
                    return Ok(Constant.MessageConstants.UserRetrievedSuccessfully, user); //Method1

                return BadRequest(Constant.MessageConstants.UserNotFound);
            }
            catch(Exception ex)
            {
                return BadRequest(Constant.MessageConstants.ErrorRetrievingUser, ex);
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
                    return Ok(Constant.MessageConstants.UserDeletedSuccessfully);

                return BadRequest(Constant.MessageConstants.UserNotFound);
            }
            catch(Exception ex)
            {
                return BadRequest(Constant.MessageConstants.ErrorDeletingUser, ex);
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
                    return Ok(Constant.MessageConstants.UserUpdatedSuccessfully, user); //Method1

                return BadRequest(Constant.MessageConstants.UserNotFound);
            }
            catch(Exception ex)
            {
                return BadRequest(Constant.MessageConstants.ErrorUpdatingUser, ex);
            }
        }
    }
}
