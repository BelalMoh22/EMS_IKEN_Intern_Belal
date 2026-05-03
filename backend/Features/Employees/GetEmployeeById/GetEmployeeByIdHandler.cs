namespace backend.Features.Employees.GetEmployeeById
{
    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, Employee?>
    {
        private readonly IRepository<Employee> _repo;
        private readonly ICurrentUserService _currentUser;

        public GetEmployeeByIdHandler(IRepository<Employee> repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        public async Task<Employee?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            if(request.Id <= 0)
                throw new Exceptions.ValidationException(new Dictionary<string, List<string>>
                {
                    { "id", new List<string> { "Id must be a positive integer." } }
                });

            var employee = await _repo.GetByIdAsync(request.Id);
            if(employee == null)
                throw new NotFoundException($"Employee with Id {request.Id} not found.");

            // 🔒 HR and Managers cannot see Master
            if (employee.User != null && employee.User.Role == Roles.Master)
            {
                if (_currentUser.UserRole == Roles.HR.ToString() || _currentUser.UserRole == Roles.Manager.ToString())
                {
                    // If it's not their own account, hide it
                    if (employee.UserId != _currentUser.UserId)
                    {
                        throw new NotFoundException($"Employee with Id {request.Id} not found.");
                    }
                }
            }

            return employee;
        }
    }
}