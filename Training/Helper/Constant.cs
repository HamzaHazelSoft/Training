namespace Training.Helper
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

            // Failure Messages
            public const string UserCreationFailed = "User creation failed";
            public const string UserNotFound = "User not found";

            // Exception Messages
            public const string ErrorCreatingUser = "An error occurred while creating user";
            public const string ErrorRetrievingUser = "An error occurred while retrieving user";
            public const string ErrorUpdatingUser = "An error occurred while updating user";
            public const string ErrorDeletingUser = "An error occurred while deleting user";

            //Authentication Messages
            public const string InvalidOrExpiredToken = "Invalid or expired token.";
            public const string ErrorRestrictedPermission = "You do not have permission to perform this action.";

            //Pagination Messages
            public const string InvalidSorting = "Invalid sorting parameters.";
        }
    }
}
