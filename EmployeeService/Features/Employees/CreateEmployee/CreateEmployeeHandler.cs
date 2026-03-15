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

        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            await _rules.ValidateForCreateAsync(dto);

            var employee = new Employee(
                dto.FirstName,
                dto.Lastname,
                dto.NationalId,
                dto.Email,
                dto.PhoneNumber,
                dto.DateOfBirth,
                dto.Address,
                dto.Salary,
                dto.PositionId,
                dto.Status
            );

            return await _repo.AddAsync(employee);
        }
    }
}