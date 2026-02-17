namespace EmployeeService.Features.Departments.CreateDepartment
{
    public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, int>
    {
        private readonly IRepository<Department> _repo;
        private readonly IRepository<Employee> _employeeRepo;
        private readonly IDepartmentBusinessRules _rules;

        public CreateDepartmentHandler(IRepository<Department> repo, IRepository<Employee> employeeRepo, IDepartmentBusinessRules rules)
        {
            _repo = repo;
            _employeeRepo = employeeRepo;
            _rules = rules;
        }

        public async Task<int> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;

            if (dto is null)
                throw new Exceptions.ValidationException(new() {"Department data is required." });

            ValidationHelper.ValidateModel(dto);
            await _rules.ValidateAsync(dto.ManagerId);

            var newDepartment = new Department
            {
                DepartmentName = dto.DepartmentName,
                ManagerId = dto.ManagerId
            };
            var id = await _repo.AddAsync(newDepartment);

            return id;
        }
    }
}
