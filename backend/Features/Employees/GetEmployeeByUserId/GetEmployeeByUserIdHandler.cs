namespace backend.Features.Employees.GetEmployeeByUserId
{
    public class GetEmployeeByUserIdHandler : IRequestHandler<GetEmployeeByUserIdQuery, EmployeeProfileDto?>
    {
        private readonly  EmployeeRepository _repo;
        private readonly ICurrentUserService _currentUser;

        public GetEmployeeByUserIdHandler(EmployeeRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        public async Task<EmployeeProfileDto?> Handle(GetEmployeeByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId <= 0)
                throw new Exceptions.ValidationException(new Dictionary<string, List<string>>
                {
                    { "id", new List<string> { "Id must be a positive integer." } }
                });

            var profile = await _repo.GetEmployeeProfileByUserIdAsync(request.UserId);
            if (profile == null)
                throw new NotFoundException($"Employee with UserId {request.UserId} not found.");

            // 🔒 HR and Managers cannot see Master profile
            if (profile.Role == Roles.Master)
            {
                if (_currentUser.UserRole == Roles.HR.ToString() || _currentUser.UserRole == Roles.Manager.ToString())
                {
                    // Hide if not self
                    if (request.UserId != _currentUser.UserId)
                    {
                        throw new NotFoundException($"Employee with UserId {request.UserId} not found.");
                    }
                }
            }

            return profile;
        }
    }
}
