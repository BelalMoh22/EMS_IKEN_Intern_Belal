namespace EmployeeService.Features.Departments.UpdateDepartment
{
    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, int>
    {
        private readonly IRepository<Department> _repo;
        private readonly IRepository<Employee> _employeeRepo;

        public UpdateDepartmentHandler(IRepository<Department> repo, IRepository<Employee> employeeRepo)
        {
            _repo = repo;
            _employeeRepo = employeeRepo;
        }

        public async Task<int> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw new Exceptions.ValidationException("Invalid department Id.");

            var existingDepartment = await _repo.GetByIdAsync(request.id);

            if (existingDepartment == null)
                throw new NotFoundException($"Department with Id {request.id} not found.");

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.dto.DepartmentName))
                errors.Add("Department name is required.");

            // ManagerId should be valid
            if (request.dto.ManagerId <= 0)
                errors.Add("Manager is required.");
            else
            {
                var manager = await _employeeRepo.GetByIdAsync(request.dto.ManagerId);
                if (manager == null)
                    errors.Add("Manager does not exist.");
            }

            if (errors.Any())
                throw new Exceptions.ValidationException(string.Join(" | ", errors));

            // Map DTO → Existing Entity
            existingDepartment.DepartmentName = request.dto.DepartmentName;
            existingDepartment.ManagerId = request.dto.ManagerId;
            existingDepartment.IsActive = request.dto.IsActive;

            var rows = await _repo.UpdateAsync(request.id, existingDepartment);

            return rows;
        }
    }
}
