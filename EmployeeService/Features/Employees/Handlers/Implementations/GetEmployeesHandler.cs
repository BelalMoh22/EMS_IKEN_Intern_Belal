namespace EmployeeService.Features.Employees.Handlers.Implementations
{
    public class GetEmployeesHandler : IGetEmployeesHandler
    {
        private readonly IRepository<Employee> _repo;

        public GetEmployeesHandler(IRepository<Employee> repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Employee>> HandleAsync()
        { 
            var employees = (await _repo.GetAllAsync()).ToList();
            if (!employees.Any())
                throw new NotFoundException("No active employees found.");

            return employees;
        }
    }
}
