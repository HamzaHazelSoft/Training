using Microsoft.AspNetCore.Identity;

namespace Training.Extensions
{
    public static class PasswordSettingsExtension
    {
        public static IServiceCollection ConfigurePassword(this IServiceCollection service)
        {
            service.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 2; // Change length
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false; // Allow no special characters
                options.Password.RequiredUniqueChars = 0;
            });
            return service;
        }
    }
}
