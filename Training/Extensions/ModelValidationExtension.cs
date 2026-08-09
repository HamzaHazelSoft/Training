using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserManagementSystem.Helper;
using UserManagementSystem.DTOs;

namespace UserManagementSystem.Extensions
{
    public static class ModelValidationExtension
    {
        public static IServiceCollection ValidateInvalidModel(this IServiceCollection services) {

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var response = new ResponseDTO<object>
                    {
                        Success = false,
                        Message = "Validations frequired fields are empty",
                        Data = null,
                        Errors = context.ModelState
                                .Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList()
                    };

                    return new BadRequestObjectResult(response);
                };
            });


            return services;
        }
    }
}
