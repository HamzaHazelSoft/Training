namespace Training.Helper
{
    public class Response<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public List<string> Errors { get; set; } = new();

        // Success with data
        public static Response<T> SuccessResponse(string message, T? data)
        {
            return new Response<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        // Success without data
        public static Response<T> SuccessResponse(string message)
        {
            return new Response<T>
            {
                Success = true,
                Message = message,
                Data = default
            };
        }

        // Failure with message only
        public static Response<T> FailureResponse(string message)
        {
            return new Response<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }

        // Failure with exception
        public static Response<T> FailureResponse(string message, Exception ex)
        {
            List<string> errors = new List<string> { ex.Message, ex.InnerException?.Message };


            return new Response<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors
            };
        }
    }
}