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
    public class UserController : ControllerBase
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
        public IActionResult Create([FromBody]User user)
        {
            ApiResponse<User> response;

            try
            {
                bool status = _userService.AddUser(user);

                if (status)
                {
                    response = new ApiResponse<User>
                    {
                        Payload = new Payload<User>
                        {
                            Items = new Item<User> { Data = user },
                            Status = 1,
                            Message = "User created successfully"
                        }
                    };
                }
                else
                {
                    response = new ApiResponse<User>
                    {
                        Payload = new Payload<User>
                        {
                            Items = new Item<User> { Data = null },
                            Status = 0,
                            Message = "User creation failed"
                        }
                    };
                }
            }
            catch (Exception e)
            {
                response = new ApiResponse<User>
                {
                    Payload = new Payload<User>
                    {
                        Items = new Item<User> { Data = null },
                        Status = 0,
                        Message = "An error occurred while creating the user",
                        Errors = new List<string> { e.Message }
                    }
                };
            }

            return Ok(response);
        }


        // Returns a user by its Id and id recieve as Route Param
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            ApiResponse<User> response;
            try
            {
                User user = _userService.GetUserById(id);

                if (user != null)
                {
                    response = new ApiResponse<User>
                    {
                        Payload = new Payload<User>
                        {
                            Items = new Item<User> { Data = user },
                            Status = 1,
                            Message = "User retrieved successfully"
                        }
                    };

                }
                else
                {
                    response = new ApiResponse<User>
                    {
                        Payload = new Payload<User>
                        {
                            Items = new Item<User> { Data = null },
                            Status = 0,
                            Message = "User not found"
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                response = new ApiResponse<User>
                {
                    Payload = new Payload<User>
                    {
                        Items = new Item<User> { Data = null },
                        Status = 0,
                        Message = "An error occurred while retrieving the user",
                        Errors = new List<string> { ex.Message }
                    }
                };
            }

            return Ok(response);
        }

        // Deletes a user by Id

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            ApiResponse<User> response;
            try {
                bool status = _userService.DeleteUserById(id);
                if (status)
                {
                    response = new ApiResponse<User>
                    {
                        Payload = new Payload<User>
                        {
                            Items = new Item<User> { Data = null },
                            Status = 1,
                            Message = "User deleted successfully"
                        }
                    };
                }
                else
                {
                    response = new ApiResponse<User>
                    {
                        Payload = new Payload<User>
                        {
                            Items = new Item<User> { Data = null },
                            Status = 0,
                            Message = "User not found"
                        }
                    };
                }
            }
            catch (Exception ex) {
                response = new ApiResponse<User>
                {
                    Payload = new Payload<User>
                    {
                        Items = new Item<User> { Data = null },
                        Status = 0,
                        Message = "An error occurred while deleting the user",
                        Errors = new List<string> { ex.Message }
                    }
                };
            }

            return Ok(response);
        }

        // Updates an existing user

        [HttpPut("{id}")]
        public IActionResult Update(string id, UserDTO User)
        {
            ApiResponse<User> response;
            try
            {
                bool status = _userService.UpdateUserById(id, User);
                if (status)
                {
                    response = new ApiResponse<User>
                    {
                        Payload = new Payload<User>
                        {
                            Items = new Item<User> { Data = null },
                            Status = 1,
                            Message = "User updated successfully"
                        }
                    };
                }
                else
                {
                    response = new ApiResponse<User>
                    {
                        Payload = new Payload<User>
                        {
                            Items = new Item<User> { Data = null },
                            Status = 0,
                            Message = "User not found"
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                response = new ApiResponse<User>
                {
                    Payload = new Payload<User>
                    {
                        Items = new Item<User> { Data = null },
                        Status = 0,
                        Message = "An error occurred while updating the user",
                        Errors = new List<string> { ex.Message }
                    }
                };
            }

            return Ok(response);
        }
    }
}
