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

            var newDepartment = new Department
            {
                DepartmentName = dto.DepartmentName,
                ManagerId = dto.ManagerId,
                IsActive = dto.IsActive
            };

            var id = await _repo.AddAsync(newDepartment);

            return id;
        }
    }
}
