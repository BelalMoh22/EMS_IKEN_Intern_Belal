namespace EmployeeService.Infrastructure.BusinessRules.Positions
{
    public interface IPositionBusinessRules
    {
        Task ValidateAsync(decimal minSalary, decimal maxSalary, int departmentId);
    }
}
