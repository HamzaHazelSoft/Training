using Microsoft.AspNetCore.Identity;
using static UserManagementSystem.Helper.Constant;

namespace UserManagementSystem.Extensions
{
    public static class PasswordSettingsExtension
    {

        public static IServiceCollection ConfigurePassword(this IServiceCollection service, IConfiguration passwordConfigurations)
        {

            var passwordSettings = passwordConfigurations.GetSection(ConfigurationConstants.PasswordSettings);

            service.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = passwordSettings.GetValue<int>(ConfigurationConstants.RequiredLength);
                options.Password.RequireDigit = passwordSettings.GetValue<bool>(ConfigurationConstants.RequireDigit);
                options.Password.RequireLowercase = passwordSettings.GetValue<bool>(ConfigurationConstants.RequireLowercase);
                options.Password.RequireUppercase = passwordSettings.GetValue<bool>(ConfigurationConstants.RequireUppercase);
                options.Password.RequireNonAlphanumeric = passwordSettings.GetValue<bool>(ConfigurationConstants.RequireNonAlphanumeric);
                options.Password.RequiredUniqueChars = passwordSettings.GetValue<int>(ConfigurationConstants.RequiredUniqueChars);
            });
            return service;
        }
    }
}
