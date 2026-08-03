using Microsoft.AspNetCore.Mvc;
using Training.Helper;

namespace Training.Controllers;

public class BaseController : ControllerBase
{
    protected IActionResult Object<T>(T data, string message)
    {
        return Ok(new ApiResponse<T>
        {
            Payload = new Payload<T>
            {
                Status = 1,
                Message = message,
                Items = new Item<T>
                {
                    Data = data
                }
            }
        });
    }

    protected IActionResult Object(string message)
    {
        return BadRequest(new ApiResponse<object>
        {
            Payload = new Payload<object>
            {
                Message = message,
                Items = null
            }
        });
    }


    protected IActionResult Object(Exception ex, string message)
    {
        return Ok(5new ApiResponse<object>
        {
            Payload = new Payload<object>
            {
                Message = message,
                Items = null,
                Errors = new List<string>
                {
                    ex.Message
                }
            }
        });
    }
}