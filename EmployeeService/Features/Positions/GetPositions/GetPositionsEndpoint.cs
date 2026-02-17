namespace EmployeeService.Features.Positions.GetPositions
{
    public static class GetPositionsEndpoint
    {
        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", async ([FromServices] IMediator mediator) =>
            {
                var command = new GetPositionsQuery();
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }).WithName("GetPositions").WithTags("Positions");
        }
    }
}
