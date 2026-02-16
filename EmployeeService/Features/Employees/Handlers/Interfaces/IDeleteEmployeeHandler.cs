namespace EmployeeService.Features.Employees.Handlers.Interfaces
{
    public interface IDeleteEmployeeHandler
    {
        Task<int> HandleAsync(int id);
    }
}