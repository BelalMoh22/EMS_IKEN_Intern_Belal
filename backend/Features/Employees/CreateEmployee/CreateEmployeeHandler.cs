namespace backend.Features.Employees.CreateEmployee
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IRepository<Employee> _repo;
        private readonly UserRepository _userRepository;
        private readonly IRepository<Position> _positionRepository;
        private readonly IEmployeeBusinessRules _rules;
        private readonly ICurrentUserService _currentUser;

        public CreateEmployeeHandler(IRepository<Employee> _repo, UserRepository _userRepository, IRepository<Position> _positionRepository, IEmployeeBusinessRules _rules, ICurrentUserService currentUser)
        {
            this._repo = _repo;
            this._userRepository = _userRepository;
            this._positionRepository = _positionRepository;
            this._rules = _rules;
            this._currentUser = currentUser;
        }
        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            await _rules.ValidateForCreateAsync(dto);

            var position = await _positionRepository.GetByIdAsync(dto.PositionId);

            // 🔒 Master can ONLY create HR accounts
            if (_currentUser.UserRole == Roles.Master.ToString())
            {
                if (dto.Role != Roles.HR)
                {
                    throw new Exceptions.ValidationException(new Dictionary<string, List<string>>
                    {
                        { "role", new List<string> { "Master can only create HR accounts." } }
                    });
                }
            }
            
            // If the DTO specifies HR, we keep it. Otherwise, we derive from Position.IsManager
            var role = dto.Role == Roles.HR ? Roles.HR : 
                       (position != null && position.IsManager) ? Roles.Manager : Roles.Employee;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var newUser = new User
            {
                Username = dto.Username,
                PasswordHash = hashedPassword,
                Role = role,
                MustChangePassword = true
            };

            var userId = await _userRepository.AddAsync(newUser);

            var employee = new Employee(
                dto.FirstName,
                dto.Lastname,
                dto.NationalId,
                dto.Email,
                dto.PhoneNumber,
                dto.DateOfBirth,

                dto.Salary,
                dto.PositionId,
                userId,
                dto.Status
            );

            return await _repo.AddAsync(employee);
        }
    }
}