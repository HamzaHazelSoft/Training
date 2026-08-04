
using Microsoft.AspNetCore.Mvc;
using Training.Helper;
namespace Training.Controllers;

public class BaseController : ControllerBase
{

        //Use protected here because i want Ye method sirf isi class aur iski child classes use karein hain.
        protected OkObjectResult Ok<T>(string message, T? data)
        {
            return (OkObjectResult)Ok(Response<T>.SuccessResponse(message, data));
        }

        protected BadRequestObjectResult BadRequest(string message)
        {
            return (BadRequestObjectResult)BadRequest(Response<object>.FailureResponse(message));
        }

        protected BadRequestObjectResult BadRequest(string message, Exception ex)
        {
            return (BadRequestObjectResult)BadRequest(Response<object>.FailureResponse(message,ex));
        }

}