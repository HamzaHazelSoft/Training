using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Training.Helper;


/*

builder.Services.AddJwtAuthentication(builder.Configuration);

Ye JWT token ko validate nahi karta.
Ye sirf Dependency Injection (DI) mein Authentication services register aur JWT Authentication middleware ko configure karta hai (issuer, audience, secret key aur events waghera).

Baad mein jab request aati hai aur:

app.UseAuthentication();

chalti hai, tab ye configuration use hoti hai aur token actually validate hota hai. 

*/
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

                options.Events = new JwtBearerEvents //JwtBearerEvents is a class that defines a set of events triggered during JWT validation
                {
                   OnChallenge = async context => //A specific event triggered right before the API returns a 401 Unauthorized during Authentication
                   {
                       context.HandleResponse(); // Prevent the ASP.NET CORE default 401 response
                       Response<string> response = Response<string>.FailureResponse(Constant.MessageConstants.InvalidOrExpiredToken);
                       await context.Response.WriteAsJsonAsync(response);
                   },
                   
                   OnForbidden = async context => //This specific event triggered right before the API return 403 Forbidden
                   {
                       Response<string> response = Response<string>.FailureResponse(Constant.MessageConstants.ErrorRestrictedPermission);
                       await context.Response.WriteAsJsonAsync(response);
                   }
                };

            });

            return services;
        }
    }
}

/*
 Authentication middleware user ki identity establish karta hai, aur Authorization middleware us identity ko use karke permissions check karta hai. 
Agar access deny ho jaye, to Authorization middleware current Authentication scheme (JWT Bearer) ko OnForbidden event execute karne ke liye bolta hai.
 */