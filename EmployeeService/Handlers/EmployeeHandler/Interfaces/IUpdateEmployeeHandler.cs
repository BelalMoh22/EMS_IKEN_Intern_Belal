namespace EmployeeService.Handlers.EmployeeHandler.Interfaces
{
    public interface IUpdateEmployeeHandler
    {
        Task<int> HandleAsync(int id, UpdateEmployeeDTO employee);
    }
}