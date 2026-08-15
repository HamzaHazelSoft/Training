using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog.Core;
using UserManagementSystem.DTOs;
using UserManagementSystem.Helper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static UserManagementSystem.Helper.Constant;

namespace UserManagementSystem.Extensions
{
    public static class ModelValidationExtension
    {
        public static IServiceCollection ValidateInvalidModel(this IServiceCollection services) {

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ApiBehaviorOptions>>();

                    logger.LogWarning(
                        "Request validation failed. Method: {Method}, Path: {Path}, Errors: {Errors}",
                         context.HttpContext.Request.Method,
                         context.HttpContext.Request.Path,
                         string.Join(" | ", errors));

                    var response = new ResponseDTO<object>
                    {
                        Success = false,
                        Message = MessageConstants.ValidationsOrRequiredFieldIssues,
                        Data = null,
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });


            return services;
        }
    }
}
