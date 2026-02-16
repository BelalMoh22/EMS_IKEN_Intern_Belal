using Microsoft.Extensions.Logging;

namespace EmployeeService.Handlers.EmployeeHandler.Implementations
{
    public class CreateEmployeeHandler : ICreateEmployeeHandler
    {
        private readonly IEmployeeRepo _repo;

        public CreateEmployeeHandler(IEmployeeRepo repo)
        {
            _repo = repo;
        }

        public async Task<int> HandleAsync(CreateEmployeeDTO employee)
        {
            if (employee is null)
                throw new CustomExceptions.ValidationException("Employee data is required.");

            var id = await _repo.AddAsync(employee);

            return id;
        }
    }
}
