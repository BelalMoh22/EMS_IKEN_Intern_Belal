namespace EmployeeService.Features.Positions.CreatePosition
{
    public static class CreatePositionEndpoint
    {
        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", async ([FromBody] CreatePositionCommand command, [FromServices] IMediator mediator) =>
            {
                var id = await mediator.Send(command);

                var response = ApiResponse<int>.SuccessResponse(id, "Position created successfully");

                return Results.Created($"/position/{id}", response);
            }).WithName("CreatePosition").WithTags("Positions");
        }
    }
}
