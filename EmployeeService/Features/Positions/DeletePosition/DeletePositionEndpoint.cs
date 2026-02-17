namespace EmployeeService.Features.Positions.DeletePosition
{
    public static class DeletePositionEndpoint
    {
        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/{id:int}", async ([FromRoute] int id, [FromServices] IMediator mediator) =>
            {
                var command = new DeletePositionCommand(id);
                var result = await mediator.Send(command);
                var response = ApiResponse<int>.SuccessResponse(id, "Position deleted successfully");
                return Results.Ok(response);
            }).WithName("DeletePosition").WithTags("Positions");
        }
    }
}
