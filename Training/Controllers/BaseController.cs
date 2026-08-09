using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.DTOs;

namespace UserManagementSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        // Success response with message + data
        protected OkObjectResult Ok<T>(string message, T? data)
        {
            return base.Ok(ResponseDTO<T>.SuccessResponse(message, data));
        }

        // Failure response with exception
        protected BadRequestObjectResult BadRequest(string message, Exception ex)
        {
            return base.BadRequest(ResponseDTO<object>.FailureResponse(message, ex));
        }

        // Override ControllerBase.Ok(object)
        public override OkObjectResult Ok(object? value)
        {
            if (value != null &&
                value.GetType().IsGenericType &&
                value.GetType().GetGenericTypeDefinition() == typeof(ResponseDTO<>))
            {
                return base.Ok(value);
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
                // Already wrapped
                return base.BadRequest(error);
            }

            // Convert string or object into standard response
            string message = error as string ?? error?.ToString() ?? string.Empty;

            return base.BadRequest(ResponseDTO<object>.FailureResponse(message));
        }
    }
}