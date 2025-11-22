namespace ITI.Gymunity.FP.APIs.Errors
{
    public class ApiResponse(int statusCode, string message = null!)
    {
        public int StatusCode { get; } = statusCode;
        public string? Message { get; } = message ?? GetDefaultMessageForStatusCode(statusCode);

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Resource Not Found",
                405 => "Not allowed",
                500 => "Internal Server Error",
                _ => null!
            };
        }
    }
}
