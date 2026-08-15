namespace UserManagementSystem.Helper
{
    public class Constant
    {
        public static string FailedToSetPassword(IConfiguration configuration)
        {
            var passwordSettingsConfigurations = configuration.GetSection("PasswordSettings");
            return
                $"Unable to set the password. Password must be at least " +
                $"{passwordSettingsConfigurations["RequiredLength"]} characters long" +
                $"{(bool.Parse(passwordSettingsConfigurations["RequireDigit"]!)
                    ? ", contain at least one digit"
                    : "")}" +
                $"{(bool.Parse(passwordSettingsConfigurations["RequireLowercase"]!)
                    ? ", one lowercase letter"
                    : "")}" +
                $"{(bool.Parse(passwordSettingsConfigurations["RequireUppercase"]!)
                    ? ", one uppercase letter"
                    : "")}" +
                $"{(bool.Parse(passwordSettingsConfigurations["RequireNonAlphanumeric"]!)
                    ? ", one special character"
                    : "")}.";
        }
        public static string InvalidPhoneNumber(int min,int max,string displayFormat)
        {
            return min == max
                ? $"Phone number must be exactly {min} digits long and must be in {displayFormat} format."
                : $"Phone number must be between {min} and {max} digits long and must be in {displayFormat} format.";
        }

        public static class ConfigurationConstants
        {
            // Configuration Sections
            public const string PasswordSettings = "PasswordSettings";
            public const string PhoneNumberSettings = "PhoneNumberSettings";
            public const string Jwt = "Jwt";

            // Password Settings Keys
            public const string RequiredLength = "RequiredLength";
            public const string RequireDigit = "RequireDigit";
            public const string RequireLowercase = "RequireLowercase";
            public const string RequireUppercase = "RequireUppercase";
            public const string RequireNonAlphanumeric = "RequireNonAlphanumeric";
            public const string RequiredUniqueChars = "RequiredUniqueChars";

            // Phone Number Settings Keys
            public const string MinimumLength = "MinimumLength";
            public const string MaximumLength = "MaximumLength";
            public const string Format = "Format";
            public const string DisplayFormat = "DisplayFormat";

            // JWT Settings Keys
            public const string Key = "Key";
            public const string Issuer = "Issuer";
            public const string Audience = "Audience";
            public const string ExpiryHours = "ExpiryHours";
        }
        public static class MessageConstants
        {
            // Success Messages
            public const string UserCreatedSuccessfully = "User created successfully";
            public const string UserRetrievedSuccessfully = "User retrieved successfully";
            public const string UserUpdatedSuccessfully = "User updated successfully";
            public const string UserDeletedSuccessfully = "User deleted successfully";
            public const string EmailConfirmedSuccessfully = "Email confirmed successfully";
            public const string LoginSuccessful = "Login successful";
            public const string PasswordSetSuccessfully = "Password set successfully";

            // Failure Messages
            public const string UserCreationFailed = "User creation failed";
            public const string UserNotFound = "User not found";
            public const string UserAlreadyExists = "User already exists";
            public const string UsernameAlreadyTaken = "Username has already been taken";
            public const string InvalidUsernameOrPassword = "Incorrect username or password";
            public const string FailedToUpdateUser = "Failed to update user";
            public const string InvalidConfirmationLink = "Invalid confirmation link";
            public const string ValidationsOrRequiredFieldIssues = "Validations failed or required fields are empty";
            public const string InvalidRolesEntered = "Invalid role entered";
            public const string EmailNotConfirmed = "Please confirm your email before logging in.";


            // Exception Messages
            public const string ErrorCreatingUser = "An error occurred while creating user";
            public const string ErrorRetrievingUser = "An error occurred while retrieving user";
            public const string ErrorUpdatingUser = "An error occurred while updating user";
            public const string ErrorDeletingUser = "An error occurred while deleting user";
            public const string ErrorRegisteringUser = "An error occurred while registering user.";
            public const string ErrorSettingPassword = "An error occurred while setting password.";
            public const string ErrorConfirmingEmail = "An error occurred while confirming email.";
            public const string ErrorLoggingIn = "An error occurred while logging in user.";

            //Authentication Messages
            public const string AuthenticationFailed = "Authentication failed. Please log in again.";
            public const string AccessDenied = "You do not have permission to access this resource.";

            //Pagination Messages
            public const string InvalidSorting = "Invalid sorting parameters.";
            public const string InvalidCurrentPage = "CurrentPage must be greater than 0.";
            public const string InvalidPageSize = "PageSize must be greater than 0.";

            // Password Messages
            public const string InvalidPasswordResetToken = "The password reset token is invalid or has expired.";
            public const string InvalidUserId = "The provided user ID is invalid.";

        }
    }
}
