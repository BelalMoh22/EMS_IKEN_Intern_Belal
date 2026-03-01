namespace EmployeeService.Features.Auth.Refresh
{
    public static class RefreshEndpoint
    {
        public static RouteGroupBuilder MapEndpoint(this RouteGroupBuilder app)
        {
            app.MapPost("/refresh", async (RefreshTokenCommand command, IMediator mediator) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            }).WithTags("Auth");

            return app;
        }
    }
}
