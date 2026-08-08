
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Security;
using System.Reflection.Metadata.Ecma335;
using Training.DTOs;
using Training.Helper;
using Training.Models;
using Training.Services.Mail;
using Training.Services;
using static Training.Helper.Constant;

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
                return Ok(MessageConstants.UserRetrievedSuccessfully, response);
            }
            catch(Exception ex)
            {
                return BadRequest(MessageConstants.InvalidSorting,ex);
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
                    return Ok(MessageConstants.UserCreatedSuccessfully, user); //Method1

                return BadRequest(MessageConstants.UserCreationFailed);
            }
            catch(Exception ex)
            {
                return BadRequest(MessageConstants.ErrorCreatingUser, ex);
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _userService.GetUserById(id);

                if (user != null)
                    return Ok(MessageConstants.UserRetrievedSuccessfully, user); //Method1

                return BadRequest(MessageConstants.UserNotFound);
            }
            catch(Exception ex)
            {
                return BadRequest(MessageConstants.ErrorRetrievingUser, ex);
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
                    return Ok(MessageConstants.UserDeletedSuccessfully);

                return BadRequest(MessageConstants.UserNotFound);
            }
            catch(Exception ex)
            {
                return BadRequest(MessageConstants.ErrorDeletingUser, ex);
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
                    return Ok(MessageConstants.UserUpdatedSuccessfully, user); //Method1

                return BadRequest(MessageConstants.UserNotFound);
            }
            catch(Exception ex)
            {
                return BadRequest(MessageConstants.ErrorUpdatingUser, ex);
            }
        }
    }
}
