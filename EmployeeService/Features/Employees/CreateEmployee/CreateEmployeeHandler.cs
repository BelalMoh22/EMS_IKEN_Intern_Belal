namespace EmployeeService.Features.Employees.CreateEmployee
{
    public class CreateEmployeeHandler: IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IRepository<Employee> _repo;
        private readonly IRepository<Position> _positionRepo;

        public CreateEmployeeHandler(
            IRepository<Employee> repo,
            IRepository<Position> positionRepo)
        {
            _repo = repo;
            _positionRepo = positionRepo;
        }

        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;

            if (dto is null)
                throw new Exceptions.ValidationException("Employee data is required.");

            var errors = new List<string>();

            if (dto.Salary <= 0)
                errors.Add("Salary must be greater than 0.");

            if (dto.DateOfBirth >= DateTime.UtcNow)
                errors.Add("Date of birth must be in the past.");


            if (dto.HireDate > DateTime.UtcNow)
                errors.Add("Hire date cannot be in the future.");

            if (dto.HireDate < dto.DateOfBirth)
                errors.Add("Hire date cannot be before date of birth.");

            if (string.IsNullOrWhiteSpace(dto.Email) ||
                !System.Text.RegularExpressions.Regex.IsMatch(
                    dto.Email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errors.Add("Invalid email format.");
            }

            // PositionId Validation
            var position = await _positionRepo.GetByIdAsync(dto.PositionId);
            if (position == null)
                errors.Add("Position does not exist.");

            // Status Validation (Enum)
            if (!Enum.IsDefined(typeof(EmployeeStatus), dto.Status))
                errors.Add("Invalid employee status.");

            if (errors.Any())
                throw new Exceptions.ValidationException(string.Join(" | ", errors));

            var newEmployee = new Employee
            {
                FirstName = dto.FirstName,
                Lastname = dto.Lastname,
                NationalId = dto.NationalId,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Address = dto.Address,
                Salary = dto.Salary,
                HireDate = dto.HireDate,
                Status = dto.Status,
                PositionId = dto.PositionId,
                CreatedAt = DateTime.UtcNow
            };

            var id = await _repo.AddAsync(newEmployee);

            return id;
        }
    }

}
