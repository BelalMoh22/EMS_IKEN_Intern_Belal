namespace EmployeeService.Domain.Common
{
    public static class ValidationHelper
    {
        public static List<string> ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            var errors = new List<string>();

            var isValid = Validator.TryValidateObject(model,context,results,true);
            if (!isValid)
                errors = results.Select(r => r.ErrorMessage).Where(msg => msg != null).Cast<string>().ToList();

            return errors;
        }
    }
}