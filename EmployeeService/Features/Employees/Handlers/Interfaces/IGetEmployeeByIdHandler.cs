namespace EmployeeService.Features.Employees.Handlers.Interfaces
{
    public interface IGetEmployeeByIdHandler
    {
        Task<Employee> HandleAsync(int id);
    }
}