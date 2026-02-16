namespace EmployeeService.Handlers.EmployeeHandler.Interfaces
{
    public interface IGetEmployeesHandler
    {
            Task<IEnumerable<Employee>> HandleAsync();
    }
}
