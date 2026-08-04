using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Training.Extensions
{
    public static class ServiceExtensions
    {

        //ya this OOP wala nhi ha, basically idr this k mtlb ha is method ko IServiceCollection pr call hona ki permission do 
        //Rules for extension : class should be static, method should be static, first parameter should be this and type of the class you want to extend
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Token verification rules
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"], 

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)) // Secret key for signing the token
                };
            });

            return services;
        }
    }
}