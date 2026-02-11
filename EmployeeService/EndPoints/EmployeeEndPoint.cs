namespace EmployeeService.EndPoints
{
    public static class EmployeeEndPoint
    {
        public static RouteGroupBuilder MapEmployeeEndPoint(this RouteGroupBuilder group)
        // this method will be used to map all employee related endpoints to the group
        {
            group.MapGet("/", async (IEmployeeRepo repo,ILoggerFactory loggerFactory) => {
                var logger = loggerFactory.CreateLogger("EmployeeEndPoint");

                var employees = await repo.GetActiveEmployeesAsync();

                if (!employees.Any())
                {
                    logger.LogInformation("No active employees found.");
                    return Results.NotFound();
                }
                return Results.Ok(employees);

            }).WithTags("Employee").WithDescription("Retrieves a list of all active employees.");

            group.MapGet("/{id:int}", async (int id, IEmployeeRepo repo, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("EmployeeEndPoint");
                if (id <= 0)
                {
                    logger.LogWarning("Invalid Id {Id}.", id);
                    return Results.BadRequest("Invalid Id.");
                }
                var employee = await repo.GetByIdAsync(id);

                if (employee is null)
                {
                    logger.LogWarning("Employee {Id} not found.", id);
                    return Results.NotFound();
                }
                return Results.Ok(employee);
            }).WithTags("Employee").WithDescription("Retrieves an employee record by its Id."); 

            group.MapPost("/", async (CreateEmployeeDTO dto,IEmployeeRepo repo,ILoggerFactory loggerFactory) =>  {
                var logger = loggerFactory.CreateLogger("EmployeeEndPoint");
                if (dto is null)
                {
                    logger.LogWarning("Request body was null.");
                    return Results.BadRequest("Employee data required.");
                }

                var id = await repo.AddAsync(dto);
                return Results.Created($"/api/employees/{id}", dto);
            }).WithTags("Employee").WithDescription("Creates a new employee record.");

            group.MapPut("/{id:int}", async (int id,UpdateEmployeeDTO dto,IEmployeeRepo repo,ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("EmployeeEndPoint");
                if (id <= 0)
                {
                    logger.LogWarning("Invalid Id {Id}.", id);
                    return Results.BadRequest("Invalid Id.");
                }
                var rows = await repo.UpdateAsync(id, dto);

                if (rows == 0)
                {
                    logger.LogWarning("Employee {Id} not found for update.", id);
                    return Results.NotFound();
                }
                return Results.Ok();
 
            }).WithTags("Employee").WithDescription("Updates an existing employee record.");


            group.MapDelete("/{id:int}", async (int id,IEmployeeRepo repo,ILoggerFactory loggerFactory) => {
                var logger = loggerFactory.CreateLogger("EmployeeEndPoint");
                if (id <= 0)
                {
                    logger.LogWarning("Invalid Id {Id}.", id);
                    return Results.BadRequest("Invalid Id.");
                }

                var rows = await repo.SoftDeleteAsync(id);

                if (rows == 0)
                {
                    logger.LogWarning("Employee {Id} not found for deletion.", id);
                    return Results.NotFound();
                }
                return Results.NoContent();
            }).WithTags("Employee").WithDescription("Soft deletes an employee record.");

            return group;
        }
    }
}