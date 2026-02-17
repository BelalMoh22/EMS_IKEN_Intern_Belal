namespace EmployeeService.Features.Employees.CreateEmployee
{
    public class CreateEmployeeHandler
        : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IRepository<Employee> _repo;
        private readonly IEmployeeBusinessRules _rules;

        public CreateEmployeeHandler(IRepository<Employee> repo,IEmployeeBusinessRules rules)
        {
            _repo = repo;
            _rules = rules;
        }

        public async Task<int> Handle(CreateEmployeeCommand request,CancellationToken cancellationToken)
        {
            var dto = request.dto;
            if (dto == null)
                throw new Exceptions.ValidationException(new() { "Employee data is required." });

            ValidationHelper.ValidateModel(dto);
            await _rules.ValidateAsync(null, dto.Email,dto.NationalId,dto.Salary,dto.PositionId);

            var employee = new Employee
            {
                FirstName = dto.FirstName,
                Lastname = dto.Lastname,
                NationalId = dto.NationalId,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Address = dto.Address,
                Salary = dto.Salary,
                HireDate = dto.HireDate ?? DateTime.UtcNow,
                Status = dto.Status ?? EmployeeStatus.Active,
                PositionId = dto.PositionId,
                CreatedAt = DateTime.UtcNow
            };

            return await _repo.AddAsync(employee);
        }
    }
}