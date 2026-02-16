using Microsoft.Extensions.Logging;

namespace EmployeeService.Handlers.EmployeeHandler.Implementations
{
    public class GetEmployeeByIdHandler : IGetEmployeeByIdHandler
    {
        private readonly IEmployeeRepo _repo;

        public GetEmployeeByIdHandler(IEmployeeRepo repo)
        {
            _repo = repo;
        }

        public async Task<Employee> HandleAsync(int id)
        {
            if (id <= 0)
                throw new CustomExceptions.ValidationException("Invalid employee Id.");

            var employee = await _repo.GetByIdAsync(id);

            if (employee is null)
                throw new NotFoundException($"Employee with Id {id} not found.");

            return employee;
        }
    }
}
