namespace EmployeeService.Handlers.EmployeeHandler.Interfaces
{
    public interface ICreateEmployeeHandler
    {
        Task<int> HandleAsync(CreateEmployeeDTO employee);
    }
}