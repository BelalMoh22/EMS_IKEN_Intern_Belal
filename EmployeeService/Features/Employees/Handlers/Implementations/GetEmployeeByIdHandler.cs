namespace EmployeeService.Features.Employees.Handlers.Implementations
{
    public class GetEmployeeByIdHandler : IGetEmployeeByIdHandler
    {
        private readonly IRepository<Employee> _repo;

        public GetEmployeeByIdHandler(IRepository<Employee> repo)
        {
            _repo = repo;
        }

        public async Task<Employee> HandleAsync(int id)
        {
            if (id <= 0)
                throw new Exceptions.ValidationException("Invalid employee Id.");

            var employee = await _repo.GetByIdAsync(id);

            if (employee is null)
                throw new NotFoundException($"Employee with Id {id} not found.");

            return employee;
        }
    }
}
