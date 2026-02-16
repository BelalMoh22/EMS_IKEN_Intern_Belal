namespace EmployeeService.Handlers.EmployeeHandler.Interfaces
{
    public interface IDeleteEmployeeHandler
    {
        Task<int> HandleAsync(int id);
    }
}