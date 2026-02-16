using EmployeeService.Features.Employees.UpdateEmployee;

namespace EmployeeService.Features.Employees.Handlers.Implementations
{
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, int>
    {
        private readonly IRepository<Employee> _repo;

        public UpdateEmployeeHandler(IRepository<Employee> repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw new Exceptions.ValidationException("Invalid employee Id.");

            var existingEmployee = await _repo.GetByIdAsync(request.id);

            if (existingEmployee == null)
                throw new NotFoundException($"Employee with Id {request.id} not found.");

            // Map DTO → Existing Entity
            existingEmployee.FirstName = request.dto.FirstName;
            existingEmployee.Lastname = request.dto.Lastname;
            existingEmployee.NationalId = request.dto.NationalId;
            existingEmployee.Email = request.dto.Email;
            existingEmployee.PhoneNumber = request.dto.PhoneNumber;
            existingEmployee.DateOfBirth = request.dto.DateOfBirth;
            existingEmployee.Address = request.dto.Address;
            existingEmployee.Salary = request.dto.Salary;
            existingEmployee.HireDate = request.dto.HireDate;
            existingEmployee.Status = request.dto.Status;
            existingEmployee.PositionId = request.dto.PositionId;

            var rows = await _repo.UpdateAsync(request.id, existingEmployee);

            return rows;
        }
    }
}
