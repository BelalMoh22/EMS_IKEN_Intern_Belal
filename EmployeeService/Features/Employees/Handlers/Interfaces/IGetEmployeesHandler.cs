namespace EmployeeService.Features.Employees.Handlers.Interfaces
{
    public interface IGetEmployeesHandler
    {
            Task<IEnumerable<Employee>> HandleAsync();
    }
}
