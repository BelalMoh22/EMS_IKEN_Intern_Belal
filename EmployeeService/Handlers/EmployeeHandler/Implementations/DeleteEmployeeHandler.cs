using Microsoft.Extensions.Logging;

namespace EmployeeService.Handlers.EmployeeHandler.Implementations
{
    public class DeleteEmployeeHandler : IDeleteEmployeeHandler
    {
        private IEmployeeRepo _repo;

        public DeleteEmployeeHandler(IEmployeeRepo repo)
        {
            _repo = repo;
        }

        public async Task<int> HandleAsync(int id)
        {
            if (id <= 0)
                throw new CustomExceptions.ValidationException("Invalid employee Id.");

            var rows = await _repo.SoftDeleteAsync(id);
            if (rows == 0)
                throw new NotFoundException($"Employee with Id {id} not found.");

            return rows;
        }

    }
}
