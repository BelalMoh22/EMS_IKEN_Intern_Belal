using Microsoft.Extensions.Logging;

namespace EmployeeService.Handlers.EmployeeHandler.Implementations
{
    public class GetEmployeesHandler : IGetEmployeesHandler
    {
        private readonly IEmployeeRepo _repo;

        public GetEmployeesHandler(IEmployeeRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Employee>> HandleAsync()
        { 
            var employees = (await _repo.GetActiveEmployeesAsync()).ToList();
            if (!employees.Any())
                throw new NotFoundException("No active employees found.");

            return employees;
        }
    }
}
