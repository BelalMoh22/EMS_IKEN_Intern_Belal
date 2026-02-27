namespace EmployeeService.Features.Auth.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly UserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwt;

        public LoginHandler(UserRepository userRepository,IJwtTokenGenerator jwt)
        {
            _userRepository = userRepository;
            _jwt = jwt;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();
            errors.AddRange(ValidationHelper.ValidateModel(request.dto));

            if (errors.Any())
                throw new Exceptions.ValidationException(errors);

            var user = await _userRepository.GetByUsernameAsync(request.dto.Username);
            if (user is null)
                throw new UnauthorizedAccessException("Invalid UserName or Password.");

            var isValid = BCrypt.Net.BCrypt.Verify(request.dto.Password, user.PasswordHash);

            if (!isValid)
                throw new UnauthorizedAccessException("Invalid UserName or Password.");

            return _jwt.GenerateToken(user);
        }
    }
}