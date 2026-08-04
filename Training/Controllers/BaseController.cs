using Microsoft.AspNetCore.Mvc;
using Training.Helper;

namespace Training.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        // Success response with message + data
        protected OkObjectResult Ok<T>(string message, T? data)
        {
            return base.Ok(Response<T>.SuccessResponse(message, data));
        }

        // Failure response with exception
        protected BadRequestObjectResult BadRequest(string message, Exception ex)
        {
            return base.BadRequest(Response<object>.FailureResponse(message, ex));
        }

        // Override ControllerBase.Ok(object)
        public override OkObjectResult Ok(object? value)
        {
            if (value != null &&
                value.GetType().IsGenericType &&
                value.GetType().GetGenericTypeDefinition() == typeof(Response<>))
            {
                return base.Ok(value);
            }

            return base.Ok(Response<object>.SuccessResponse(string.Empty, value));
        }

        // Override ControllerBase.BadRequest(object)
        public override BadRequestObjectResult BadRequest(object? error)
        {
            if (error != null &&
                error.GetType().IsGenericType &&
                error.GetType().GetGenericTypeDefinition() == typeof(Response<>))
            {
                // Already wrapped
                return base.BadRequest(error);
            }

            // Convert string or object into standard response
            string message = error as string ?? error?.ToString() ?? string.Empty;

            return base.BadRequest(Response<object>.FailureResponse(message));
        }
    }
}