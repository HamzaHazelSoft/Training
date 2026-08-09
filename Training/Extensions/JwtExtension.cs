using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagementSystem.DTOs;
using static UserManagementSystem.Helper.Constant;

namespace UserManagementSystem.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers JWT Bearer authentication and configures the token validation rules.
        /// </summary>
        /// <param name="services">
        /// The application's dependency injection service collection.
        /// </param>
        /// <param name="configuration">
        /// Provides access to JWT configuration values from appsettings.json.
        /// </param>
        /// <returns>
        /// The same IServiceCollection instance so additional services can be registered.
        /// </returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,IConfiguration configuration)
        {

            // Configure JWT Bearer as the default authentication and challenge scheme.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Define the rules used to validate incoming JWT tokens.
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Token verification rules
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"], 

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };

                // Customize the responses generated during JWT authentication and authorization.
                options.Events = new JwtBearerEvents
                {
                   // Executed when authentication fails and the API is about to return 401 Unauthorized.
                    OnChallenge = async context => 
                    {
                       // Prevent the ASP.NET CORE default 401 response
                       context.HandleResponse();
                       context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        ResponseDTO<string> response = ResponseDTO<string>.FailureResponse(MessageConstants.AuthenticationFailed);
                       await context.Response.WriteAsJsonAsync(response);
                    },

                    // Executed when the user is authenticated but does not have permission
                    // to access the requested resource.
                    OnForbidden = async context => 
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        ResponseDTO<string> response = ResponseDTO<string>.FailureResponse(MessageConstants.AccessDenied);
                       await context.Response.WriteAsJsonAsync(response);
                    }
                };

            });

            return services;
        }
    }
}
