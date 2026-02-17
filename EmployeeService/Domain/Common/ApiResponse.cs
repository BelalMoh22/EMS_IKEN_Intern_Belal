namespace EmployeeService.Domain.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(string message = "")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
            };
        }

        public static ApiResponse<T> FailureResponse(IEnumerable<string> errors, string message = "")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
}