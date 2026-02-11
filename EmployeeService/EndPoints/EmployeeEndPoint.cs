using EmployeeService.Models;

namespace EmployeeService.EndPoints
{
    public static class EmployeeEndPoint
    {
        public static RouteGroupBuilder MapEmployeeEndPoint(this RouteGroupBuilder group)
        // this method will be used to map all employee related endpoints to the group
        {
            group.MapGet("/", async (IEmployeeRepo repo,ILoggerFactory loggerFactory) => {
                var logger = loggerFactory.CreateLogger("EmployeeEndPoint");
                logger.LogInformation("GET /employees started.");
                try
                {
                    var employees = await repo.GetActiveEmployeesAsync();

                    if (!employees.Any())
                    {
                        logger.LogInformation("No active employees found.");
                        return Results.NotFound();
                    }

                    logger.LogInformation("Returned {Count} employees.", employees.Count());
                    return Results.Ok(employees);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while retrieving employees.");
                    return Results.StatusCode(500);
                }
            }).WithTags("Employee").WithName("GetAllEmployees").WithDescription("Retrieves a list of all active employees.");

            group.MapGet("/{id:int}", async (int id, IEmployeeRepo repo, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("EmployeeEndPoint");
                logger.LogInformation("GET /employees/{Id} started.", id);

                if (id <= 0)
                {
                    logger.LogWarning("Invalid Id {Id}.", id);
                    return Results.BadRequest("Invalid Id.");
                }

                try
                {
                    var employee = await repo.GetByIdAsync(id);

                    if (employee is null)
                    {
                        logger.LogWarning("Employee {Id} not found.", id);
                        return Results.NotFound();
                    }
                    logger.LogInformation("Returned employee with {Id}.", employee.Id);
                    return Results.Ok(employee);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while retrieving employee {Id}.", id);
                    return Results.StatusCode(500);
                }
            }).WithTags("Employee").WithName("GetEmployeeById").WithDescription("Retrieves an employee record by its Id."); 

            group.MapPost("/", async (CreateEmployeeDTO dto,IEmployeeRepo repo,ILoggerFactory loggerFactory) =>  {
                var logger = loggerFactory.CreateLogger("EmployeeEndPoint");
                logger.LogInformation("POST /employees started.");

                if (dto is null)
                {
                    logger.LogWarning("Request body was null.");
                    return Results.BadRequest("Employee data required.");
                }

                try
                {
                    var id = await repo.AddAsync(dto);

                    logger.LogInformation("Employee created with Id {Id}.", id);

                    return Results.Created($"/api/Employee/{id}", dto);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while creating employee.");
                    return Results.StatusCode(500);
                }
            }).WithTags("Employee").WithName("CreateEmployee").WithDescription("Creates a new employee record.");

            group.MapPut("/{id:int}", async (int id,UpdateEmployeeDTO dto,IEmployeeRepo repo,ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("EmployeeEndPoint");
                logger.LogInformation("PUT /employees/{Id} started.", id);

                if (id <= 0)
                {
                    logger.LogWarning("Invalid Id {Id}.", id);
                    return Results.BadRequest("Invalid Id.");
                }

                try
                {
                    var rows = await repo.UpdateAsync(id, dto);

                    if (rows == 0)
                    {
                        logger.LogWarning("Employee {Id} not found for update.", id);
                        return Results.NotFound();
                    }

                    logger.LogInformation("Employee {Id} updated successfully.", id);

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while updating employee {Id}.", id);
                    return Results.StatusCode(500);
                }
            }).WithTags("Employee").WithName("UpdateEmployee").WithDescription("Updates an existing employee record.");

            group.MapDelete("/{id:int}", async (int id,IEmployeeRepo repo,ILoggerFactory loggerFactory) => {
                var logger = loggerFactory.CreateLogger("EmployeeEndPoint");
                logger.LogInformation("DELETE /employees/{Id} started.", id);

                if (id <= 0)
                {
                    logger.LogWarning("Invalid Id {Id}.", id);
                    return Results.BadRequest("Invalid Id.");
                }

                try
                {
                    var rows = await repo.SoftDeleteAsync(id);

                    if (rows == 0)
                    {
                        logger.LogWarning("Employee {Id} not found for deletion.", id);
                        return Results.NotFound();
                    }

                    logger.LogInformation("Employee {Id} deleted successfully.", id);

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while deleting employee {Id}.", id);
                    return Results.StatusCode(500);
                }
            }).WithTags("Employee").WithName("DeleteEmployee").WithDescription("Soft deletes an employee record.");

            return group;
        }
    }
}