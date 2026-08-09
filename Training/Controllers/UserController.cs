
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Security;
using System.Reflection.Metadata.Ecma335;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;
using UserManagementSystem.Services.Mail;
using UserManagementSystem.Services;
using static UserManagementSystem.Helper.Constant;

namespace UserManagementSystem.Controllers
{

    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService UserService) 
        {
            _userService = UserService; 
        }

        //GetUser
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequestDTO paginationRequest)
        {
            try
            {
                var response = await _userService.GetUsers(paginationRequest);
                return Ok(MessageConstants.UserRetrievedSuccessfully, response);
            }
            catch (ArgumentException ex) {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(MessageConstants.InvalidSorting, ex);
            }
        }   

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _userService.GetUserById(id);

                if (user != null)
                    return Ok(MessageConstants.UserRetrievedSuccessfully, user);

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
                bool result = await _userService.DeleteUserById(id);

                if (result)
                    return Ok(MessageConstants.UserDeletedSuccessfully,"");

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
                bool result = await _userService.UpdateUserById(id, user);

                if (result)
                    return Ok(MessageConstants.UserUpdatedSuccessfully, user); //Method1

                return BadRequest(MessageConstants.UserNotFound);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(MessageConstants.ErrorUpdatingUser, ex);
            }
        }
    }
}
