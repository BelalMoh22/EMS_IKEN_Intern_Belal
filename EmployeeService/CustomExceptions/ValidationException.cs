namespace EmployeeService.CustomExceptions
{
    public class ValidationException : AppException
    {
        public ValidationException(string message) : base(message) { }
    }

}
