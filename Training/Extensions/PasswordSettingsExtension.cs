using Microsoft.AspNetCore.Identity;

namespace UserManagementSystem.Extensions
{
    public static class PasswordSettingsExtension
    {
        public static IServiceCollection ConfigurePassword(this IServiceCollection service)
        {
            service.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8; // Change length
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true; // Allow no special characters
                options.Password.RequiredUniqueChars = 1;
            });
            return service;
        }
    }
}
