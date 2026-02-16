namespace EmployeeService.Features.Employees.EndPoints
{
    public static class EmployeeEndPoint
    {
        public static RouteGroupBuilder MapEmployeeEndPoint(this RouteGroupBuilder group) // this method will be used to map all employee related endpoints to the group
        {
            group.MapGet("/", async (IGetEmployeesHandler _handler) => {
                var employees = await _handler.HandleAsync(); 
                return Results.Ok(employees);
            }).WithTags("Employee").WithDescription("Retrieves a list of all active employees.");

            group.MapGet("/{id:int}", async (int id, IGetEmployeeByIdHandler _handler) =>
            {    
                var employee = await _handler.HandleAsync(id);
                return Results.Ok(employee);
            }).WithTags("Employee").WithDescription("Retrieves an employee record by its Id."); 

            group.MapPost("/", async (CreateEmployeeDTO dto ,ICreateEmployeeHandler _handler) =>  {
                var createdId = await _handler.HandleAsync(dto);
                return Results.Created($"/api/employees/{createdId}", dto);
            }).WithTags("Employee").WithDescription("Creates a new employee record.");

            group.MapPut("/{id:int}", async (int id ,UpdateEmployeeDTO dto,IUpdateEmployeeHandler _handler) =>
            {   
                var rows = await _handler.HandleAsync(id, dto);
                return Results.NoContent();
            }).WithTags("Employee").WithDescription("Updates an existing employee record.");

            group.MapDelete("/{id:int}", async (int id ,IDeleteEmployeeHandler _handler) => {
                var rows = await _handler.HandleAsync(id);
                return Results.NoContent();
            }).WithTags("Employee").WithDescription("Soft deletes an employee record.");

            return group;
        }
    }
}