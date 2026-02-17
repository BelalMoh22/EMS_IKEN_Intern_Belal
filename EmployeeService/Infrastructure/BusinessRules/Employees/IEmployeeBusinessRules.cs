
namespace EmployeeService.Infrastructure.BusinessRules.Employees
{
    public interface IEmployeeBusinessRules
    {
        Task ValidateAsync(int? employeeId, string email, string nationalId, decimal salary, int positionId);
    }
}