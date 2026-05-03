namespace backend.Features.Employees.DeleteEmployee
{
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, EmployeeActionResult>
    {
        private readonly IRepository<Employee> _repo;
        private readonly UserRepository _userRepo;
        private readonly IEmployeeBusinessRules _rules;
        private readonly ICurrentUserService _currentUser;

        public DeleteEmployeeHandler(IRepository<Employee> repo, UserRepository userRepo, IEmployeeBusinessRules rules, ICurrentUserService currentUser)
        {
            _repo = repo;
            _userRepo = userRepo;
            _rules = rules;
            _currentUser = currentUser;
        }

        public async Task<EmployeeActionResult> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (request.id <= 0)
                throw new Exceptions.ValidationException(new Dictionary<string, List<string>>
                {
                    { "id", new List<string> { "Id must be a positive integer." } }
                });

            var employee = await _repo.GetByIdAsync(request.id);
            if (employee == null)
                throw new NotFoundException($"Employee with Id {request.id} not found.");

            // 🔒 Master restrictions
            if (_currentUser.UserRole == Roles.Master.ToString())
            {
                // Only allow deleting HR employees
                if (employee.User == null || employee.User.Role != Roles.HR)
                    throw new UnauthorizedAccessException("Master can only delete HR employees.");

                // 🚫 Prevent self-delete
                if (employee.UserId == _currentUser.UserId)
                    throw new Exceptions.ValidationException(new Dictionary<string, List<string>>
                    {
                        { "id", new List<string> { "You cannot delete your own account." } }
                    });
            }

            // Check if manager in any department
            var deptName = await _rules.HandleManagerRemovalAsync(request.id);

            var rows = await _repo.DeleteAsync(request.id);

            // If employee has an associated user account, delete it as well
            if (rows > 0 && employee.UserId > 0)
            {
                var user = await _userRepo.GetByIdAsync(employee.UserId);
                if (user != null && !user.IsDeleted)
                {
                    await _userRepo.DeleteAsync(employee.UserId);
                }
            }

            var message = rows > 0 ? "Employee deleted successfully." : "No employee was deleted.";
            if (!string.IsNullOrEmpty(deptName))
            {
                message += $" Important: This employee was designated as the Manager for the '{deptName}' department. Upon deletion, the department manager position is now vacant. Action Required: Please designate a new manager for the '{deptName}' department to ensure administrative continuity.";
            }

            return new EmployeeActionResult(rows, message);
        }
    }
}