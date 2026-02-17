namespace EmployeeService.Features.Employees.DeleteEmployee
{
    public static class DeleteEmployeeEndPoint
    {
        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/{id:int}", async ([FromRoute] int id, [FromServices] IMediator mediator) =>
            {
                var command = new DeleteEmployeeCommand(id);
                var result = await mediator.Send(command);
                var response = ApiResponse<int>.SuccessResponse("Employee deleted successfully");
                return Results.Ok(response);
            }).WithName("DeleteEmployee").WithTags("Employees");
        }
    }
}
