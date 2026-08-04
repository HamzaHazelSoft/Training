

namespace Training.Helper
{
    public class Response<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();  

        public static Response<T> SuccessResponse(string message, T? data)
        {
            return new Response<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static Response<T> FailureResponse(string message,Exception ex)
        {
            List<string> errors = new List<string> { ex.Message };

            return new Response<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors
            };
        }
        public static Response<T> FailureResponse(string message)
        {
            {
                return new Response<T>
                {
                    Success = false,
                    Message = message,
                    Data = default
                };
            }
        }
    }
}