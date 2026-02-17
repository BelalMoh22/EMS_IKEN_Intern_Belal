using System.ComponentModel.DataAnnotations;

namespace EmployeeService.Domain.Common
{
    public static class ValidationHelper
    {
        public static void ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model,context,results,true);

            if (!isValid)
            {
                var errors = results
                    .Select(r => r.ErrorMessage)
                    .Where(msg => msg != null)
                    .Cast<string>()
                    .ToList();

                throw new Exceptions.ValidationException(errors);
            }
        }
    }

}