namespace EmployeeService.Features.Employees.Handlers.Interfaces
{
    public interface ICreateEmployeeHandler
    {
        Task<int> HandleAsync(CreateEmployeeDTO employee);
    }
}