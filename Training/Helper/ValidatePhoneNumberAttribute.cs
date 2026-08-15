using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using static UserManagementSystem.Helper.Constant;

namespace UserManagementSystem.Helper
{
    public class ValidatePhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var configuration = validationContext.GetService(typeof(IConfiguration)) as IConfiguration;
            var settings = configuration!
                .GetSection(ConfigurationConstants.PhoneNumberSettings);

            int minLength = int.Parse(settings[ConfigurationConstants.MinimumLength]!);
            int maxLength = int.Parse(settings[ConfigurationConstants.MaximumLength]!);
            string format = settings[ConfigurationConstants.Format]!;
            string displayFormat = settings[ConfigurationConstants.DisplayFormat]!;

            if (value is not string phoneNumber ||
                string.IsNullOrWhiteSpace(phoneNumber) ||
                phoneNumber.Length < minLength ||
                phoneNumber.Length > maxLength ||
                !Regex.IsMatch(phoneNumber, format))
            {
                return new ValidationResult(
                    Constant.InvalidPhoneNumber(
                        minLength,
                        maxLength,
                        displayFormat
                    )
                );
            }

            return ValidationResult.Success;
        }
    }
}
