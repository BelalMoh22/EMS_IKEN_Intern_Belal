namespace EmployeeService.Features.Auth.Login
{
    public class RefreshTokenHandler: IRequestHandler<RefreshCommand, AuthResponse>
    {
        private readonly UserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly IJwtTokenGenerator _jwt;

        public RefreshTokenHandler(UserRepository userRepository,IRefreshTokenRepository refreshRepo,IJwtTokenGenerator jwt)
        {
            _userRepository = userRepository;
            _refreshRepo = refreshRepo;
            _jwt = jwt;
        }

        public async Task<AuthResponse> Handle(RefreshCommand request,CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.dto.Username);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.dto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or Password.");
            }

            var accessToken = _jwt.GenerateToken(user);
            var refreshToken = _jwt.GenerateRefreshToken();

            await _refreshRepo.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            });

            return new AuthResponse(accessToken, refreshToken);
        }
    }
}