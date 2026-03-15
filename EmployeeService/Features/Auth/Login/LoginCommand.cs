namespace EmployeeService.Features.Auth.Login
{
    public record RefreshCommand(LoginDto dto): IRequest<AuthResponse>;
}
