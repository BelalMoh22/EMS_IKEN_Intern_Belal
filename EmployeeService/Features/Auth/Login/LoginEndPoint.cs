namespace EmployeeService.Features.Auth.Login
{
    public static class LoginEndpoint
    {
        public static RouteGroupBuilder MapEndpoint(this RouteGroupBuilder app)
        {
            app.MapPost("/login",async (LoginDto dto, IMediator mediator) =>
            {
                var command = new RefreshCommand(dto);
                var token = await mediator.Send(command);
                return Results.Ok(new { token });
            }).WithTags("Auth");

            return app;
        }
    }
}