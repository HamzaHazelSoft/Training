
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
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService UserService,ILogger<UserController> logger) 
        {
            _userService = UserService;
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.LogError(ex,"Failed to retrieve users");
                return BadRequest(ex.Message);
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
                _logger.LogError(ex, "Failed to retrieve user. UserId: {UserId}", id);

                return BadRequest(ex.Message);
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
                    return Ok(MessageConstants.UserDeletedSuccessfully);

                return BadRequest(MessageConstants.UserNotFound);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Failed to delete user. UserId: {UserId}",id);
                return BadRequest(ex.Message);
            }
        }

        // Update User
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, RegisterDTO registerDTO)
        {
            try
            {
                bool result = await _userService.UpdateUserById(id, registerDTO);
                return Ok(MessageConstants.UserUpdatedSuccessfully, registerDTO); 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Failed to update user. UserId: {UserId}, UserName: {UserName}, Email: {Email}",
                    id,registerDTO.UserName,registerDTO.Email);
                return BadRequest(ex.Message);
            }
        }
    }
}
