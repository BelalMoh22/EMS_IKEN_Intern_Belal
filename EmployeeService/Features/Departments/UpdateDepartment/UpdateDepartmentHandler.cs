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

            // Map DTO → Existing Entity
            existingDepartment.DepartmentName = request.dto.DepartmentName;
            existingDepartment.ManagerId = request.dto.ManagerId;
            existingDepartment.IsActive = request.dto.IsActive;

            var rows = await _repo.UpdateAsync(request.id, existingDepartment);

            return rows;
        }
    }
}
