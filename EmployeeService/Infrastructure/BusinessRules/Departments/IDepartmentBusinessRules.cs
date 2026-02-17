namespace EmployeeService.Infrastructure.BusinessRules.Departments
{
    public interface IDepartmentBusinessRules
    {
        Task ValidateAsync(int managerId);
    }
}
