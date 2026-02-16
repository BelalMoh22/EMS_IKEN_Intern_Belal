namespace EmployeeService.Features.Employees.Handlers.Interfaces
{
    public interface IUpdateEmployeeHandler
    {
        Task<int> HandleAsync(int id, UpdateEmployeeDTO employee);
    }
}