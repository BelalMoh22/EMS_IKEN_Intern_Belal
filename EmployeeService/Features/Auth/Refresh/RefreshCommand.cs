namespace EmployeeService.Features.Auth.Refresh
{
    public record RefreshTokenCommand(string RefreshToken): IRequest<AuthResponse>;
}
