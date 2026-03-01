namespace EmployeeService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Add services to the container(DI).
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IRepository<Employee>, EmployeeRepository>();
            builder.Services.AddScoped<IRepository<Department>, DepartmentRepository>();
            builder.Services.AddScoped<IRepository<Position>, PositionRepository>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<IEmployeeBusinessRules, EmployeeBusinessRules>();
            builder.Services.AddScoped<IPositionBusinessRules, PositionBusinessRules>();
            builder.Services.AddScoped<IDepartmentBusinessRules, DepartmentBusinessRules>();

            // Use Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var config = builder.Configuration;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                        RoleClaimType = ClaimTypes.Role
                    };
                });
            // Use Authorization
            builder.Services.AddAuthorization( builder =>
            {
                builder.AddPolicy("FullCRUD", context =>
                {
                    context.RequireRole(Roles.Admin.ToString());
                });

                builder.AddPolicy("EmployeesReadOnly", context =>
                {
                    context.RequireRole(Roles.HR.ToString(), Roles.Admin.ToString(), Roles.Manager.ToString());
                });

                builder.AddPolicy("FullCRUDEmployee", context =>
                {
                    context.RequireRole(Roles.HR.ToString(), Roles.Admin.ToString());
                });
            });

            // Inject MediatR into DI Container
            builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var appName = builder.Configuration["ApplicationSettings:ApplicationName"];
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            var app = builder.Build();
            app.Logger.LogInformation("Application started successfully (Information)");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Best Order for Middleware: Exception Handling, Logging, Authentication, Authorization
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapGroup("/api/auth").MapAuthEndpoints();
            app.MapGroup("/api/employees").MapEmployeesEndpoints();
            app.MapGroup("/api/departments").MapDepartmentEndpoints().RequireAuthorization("FullCRUD");
            app.MapGroup("/api/positions").MapPositionEndpoints().RequireAuthorization("FullCRUD");

            app.Run();
        }
    }
}
