using EmployeeService.Models;
using Microsoft.Extensions.Logging;

namespace EmployeeService.Handlers.EmployeeHandler.Implementations
{
    public class UpdateEmployeeHandler : IUpdateEmployeeHandler
    {
        private readonly IEmployeeRepo _repo;

        public UpdateEmployeeHandler(IEmployeeRepo repo)
        {
            _repo = repo;
        }

        public async Task<int> HandleAsync(int id, UpdateEmployeeDTO employee)
        {
            if (id <= 0)
                throw new CustomExceptions.ValidationException("Invalid employee Id.");

            var rows = await _repo.UpdateAsync(id, employee);
            if (rows == 0)
                throw new NotFoundException($"Employee with Id {id} not found.");

            return rows;
        }
    }
}
