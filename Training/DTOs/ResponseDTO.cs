namespace UserManagementSystem.DTOs
{
    public class ResponseDTO<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public List<string> Errors { get; set; } = new();

        // Success with data
        public static ResponseDTO<T> SuccessResponse(string message, T? data)
        {
            return new ResponseDTO<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        // Success without data
        public static ResponseDTO<T> SuccessResponse(string message)
        {
            return new ResponseDTO<T>
            {
                Success = true,
                Message = message,
                Data = default
            };
        }

        // Failure with message only
        public static ResponseDTO<T> FailureResponse(string message)
        {
            return new ResponseDTO<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }

        // Failure with exception
        public static ResponseDTO<T> FailureResponse(string message, Exception ex)
        {
            List<string> errors = new List<string> { ex.Message, ex.InnerException?.Message };


            return new ResponseDTO<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors
            };
        }
    }
}