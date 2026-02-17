namespace EmployeeService.Features.Departments.CreateDepartment
{
    public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, int>
    {
        private readonly IRepository<Department> _repo;
        private readonly IRepository<Employee> _employeeRepo;

        public CreateDepartmentHandler(IRepository<Department> repo, IRepository<Employee> employeeRepo)
        {
            _repo = repo;
            _employeeRepo = employeeRepo;
        }

        public async Task<int> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;

            if (dto is null)
                throw new Exceptions.ValidationException("Department data is required.");

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.DepartmentName))
                errors.Add("Department name is required.");

            // ManagerId should be valid
            if (dto.ManagerId <= 0)
                errors.Add("Manager is required.");
            else
            {
                var manager = await _employeeRepo.GetByIdAsync(dto.ManagerId);
                if (manager == null)
                    errors.Add("Manager does not exist.");
            }

            if (errors.Any())
                throw new Exceptions.ValidationException(string.Join(" | ", errors));

            var newDepartment = new Department
            {
                DepartmentName = dto.DepartmentName,
                ManagerId = dto.ManagerId,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            var id = await _repo.AddAsync(newDepartment);

            return id;
        }
    }
}
