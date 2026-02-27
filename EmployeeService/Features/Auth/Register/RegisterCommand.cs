namespace EmployeeService.Features.Auth.Register
{
    public record RegisterCommand(RegisterDto dto) :IRequest<int>;
}
