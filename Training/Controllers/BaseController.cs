using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.DTOs;

namespace UserManagementSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        // Success response with message and data
        protected OkObjectResult Ok<T>(string message, T? data)
        {
            return base.Ok(ResponseDTO<T>.SuccessResponse(message, data)); 
        }

        // Failure response with some exception
        protected BadRequestObjectResult BadRequest(string message, Exception ex)
        {
            return base.BadRequest(ResponseDTO<object>.FailureResponse(message, ex));
        }

        // Here we are overriding ControllerBase.Ok(object)
        public override OkObjectResult Ok(object? value)
        {
            if (value != null &&
                value.GetType().IsGenericType &&
                value.GetType().GetGenericTypeDefinition() == typeof(ResponseDTO<>))
            {
                return base.Ok(value);
            }
            if(value is string stringValue)
            {
                return base.Ok(ResponseDTO<string>.SuccessResponse(stringValue));
            }

            return base.Ok(ResponseDTO<object>.SuccessResponse(string.Empty, value));
        }

        // Override ControllerBase.BadRequest(object)
        public override BadRequestObjectResult BadRequest(object? error)
        {
            if (error != null &&
                error.GetType().IsGenericType &&
                error.GetType().GetGenericTypeDefinition() == typeof(ResponseDTO<>)) 
            {
                // Already wrapped so we can directly send it to our base Controller
                return base.BadRequest(error);
            }

            if (error is string errorMessage)
            {
                return base.BadRequest(
                    ResponseDTO<object>.FailureResponse(errorMessage)
                );
            }

            string message = error?.ToString() ?? string.Empty;

            return base.BadRequest(ResponseDTO<object>.FailureResponse(message));
        }
    }
}