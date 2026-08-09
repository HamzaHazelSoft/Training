namespace UserManagementSystem.Helper
{
    public class Constant
    {
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
            public const string FailedToSetPassword = "Failed to set password. Please follow the format";
            public const string InvalidConfirmationLink = "Invalid confirmation link";


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
            public const string AuthenticationFailed = "Your session has expired or is no longer valid. Please log in again.";
            public const string AccessDenied = "You do not have permission to access this resource.";

            //Pagination Messages
            public const string InvalidSorting = "Invalid sorting parameters.";
            public const string InvalidCurrentPage = "CurrentPage must be greater than 0.";
            public const string InvalidPageSize = "PageSize must be greater than 0.";
        }
    }
}
