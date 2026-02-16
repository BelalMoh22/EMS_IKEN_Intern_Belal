namespace EmployeeService.Handlers.EmployeeHandler.Interfaces
{
    public interface IGetEmployeeByIdHandler
    {
        Task<Employee> HandleAsync(int id);
    }
}