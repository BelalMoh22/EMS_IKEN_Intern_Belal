namespace EmployeeService.Features.Employees.Handlers.Implementations
{
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, int>
    {
        private readonly IRepository<Employee> _repo;
        private readonly IRepository<Position> _positionRepo;

        public UpdateEmployeeHandler(IRepository<Employee> repo, IRepository<Position> positionRepo)
        {
            _repo = repo;
            _positionRepo = positionRepo;
        }

        public async Task<int> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw new Exceptions.ValidationException("Invalid employee Id.");

            var existingEmployee = await _repo.GetByIdAsync(request.id);

            if (existingEmployee == null)
                throw new NotFoundException($"Employee with Id {request.id} not found.");

            var errors = new List<string>();
            if (request.dto.Salary <= 0)
                errors.Add("Salary must be greater than 0.");

            if (request.dto.DateOfBirth >= DateTime.UtcNow)
                errors.Add("Date of birth must be in the past.");


            if (request.dto.HireDate > DateTime.UtcNow)
                errors.Add("Hire date cannot be in the future.");

            if (request.dto.HireDate < request.dto.DateOfBirth)
                errors.Add("Hire date cannot be before date of birth.");

            if (string.IsNullOrWhiteSpace(request.dto.Email) ||
                !System.Text.RegularExpressions.Regex.IsMatch(
                    request.dto.Email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errors.Add("Invalid email format.");
            }

            // PositionId Validation
            var position = await _positionRepo.GetByIdAsync(request.dto.PositionId);
            if (position == null)
                errors.Add("Position does not exist.");

            // Status Validation (Enum)
            if (!Enum.IsDefined(typeof(EmployeeStatus), request.dto.Status))
                errors.Add("Invalid employee status.");

            if (errors.Any())
                throw new Exceptions.ValidationException(string.Join(" | ", errors));

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
