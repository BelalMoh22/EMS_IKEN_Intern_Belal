namespace EmployeeService.Features.Positions.GetPositionById
{
    public static class GetPositionByIdEndpoint
    {
        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{id:int}", async ([FromServices] IMediator mediator, [FromRoute] int id) =>
            {
                var command = new GetPositionByIdQuery(id);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }).WithName("GetPositionById").WithTags("Positions");
        }
    }
}
