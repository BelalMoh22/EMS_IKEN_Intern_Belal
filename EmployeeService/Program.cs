namespace EmployeeService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepository>();
            builder.Services.AddScoped<IGenericRepo<Department>, DepartmentRepository>();
            builder.Services.AddScoped<IGenericRepo<Position>, PositionRepository>();
            // Use Auth
            builder.Services.AddAuthorization();

            // Add DbConnection and ApplicationName
            var appName = builder.Configuration["ApplicationSettings:ApplicationName"];
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            var app = builder.Build();
            // Log application startup information
            app.Logger.LogTrace("Application is starting (Trace)");
            app.Logger.LogInformation("Application started successfully (Information)");
            app.Logger.LogWarning("This is a sample warning during startup (Warning)");

            try
            {
                if (string.IsNullOrEmpty(builder.Configuration.GetConnectionString("DefaultConnection")))
                {
                    throw new Exception("Connection string missing");
                }
            }
            catch (Exception ex)
            {
                app.Logger.LogCritical(ex, "Critical error during startup");
            }

            // Log application name and connection string for debugging
            //app.Logger.LogInformation("App Name: {AppName}", appName);
            //app.Logger.LogInformation("Connection String: {ConnectionString}", connectionString);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseAuthorization();

            // /health endpoint 
            //app.MapGet("/api/health", () => Results.Ok("First EndPoint From EMS"));
            app.MapGroup("/api/employees").MapEmployeeEndPoint();
            app.Run();

        }
    }
}
