using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Training.Helper;

namespace Training.Extensions
{
    public static class ModelValidationExtension
    {
        public static IServiceCollection ValidateInvalidModel(this IServiceCollection services) {

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var response = new Response<object>
                    {
                        Success = false,
                        Message = "Validation Failed",
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
