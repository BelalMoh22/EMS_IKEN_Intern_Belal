namespace EmployeeService.Features.Employees.Handlers.Implementations
{
    public class DeleteEmployeeHandler : IDeleteEmployeeHandler
    {
        private IRepository<Employee> _repo;

        public DeleteEmployeeHandler(IRepository<Employee> repo)
        {
            _repo = repo;
        }

        public async Task<int> HandleAsync(int id)
        {
            if (id <= 0)
                throw new Exceptions.ValidationException("Invalid employee Id.");

            var rows = await _repo.DeleteAsync(id);
            if (rows == 0)
                throw new NotFoundException($"Employee with Id {id} not found.");

            return rows;
        }

    }
}
