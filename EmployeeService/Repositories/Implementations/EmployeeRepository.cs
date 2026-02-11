namespace EmployeeService.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepo
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(IConfiguration configuration,ILogger<EmployeeRepository> logger)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            _logger = logger;
        }

        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        {
            var sql = "SELECT * FROM Employees WHERE IsDeleted = 0";

            _logger.LogDebug("SQL: Retrieving active employees.");

            return await _connection.QueryAsync<Employee>(sql);
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Employees WHERE Id = @Id";

            _logger.LogDebug("SQL: Retrieving employee with Id {Id}.", id);

            return await _connection.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(CreateEmployeeDTO dto)
        {
            var sql = @"
                INSERT INTO Employees
                (FirstName, Lastname, NationalId, Email, PhoneNumber,
                 DateOfBirth, Address, Salary, HireDate, Status, PositionId)
                VALUES
                (@FirstName, @Lastname, @NationalId, @Email, @PhoneNumber,
                 @DateOfBirth, @Address, @Salary, @HireDate, @Status, @PositionId);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            _logger.LogDebug("SQL: Inserting employee with Email {Email}.", dto.Email);

            return await _connection.QuerySingleAsync<int>(sql, dto);
        }

        public async Task<int> UpdateAsync(int id, UpdateEmployeeDTO dto)
        {
            var sql = @"
                UPDATE Employees
                SET
                    FirstName = @FirstName,
                    Lastname = @Lastname,
                    NationalId = @NationalId,
                    Email = @Email,
                    PhoneNumber = @PhoneNumber,
                    DateOfBirth = @DateOfBirth,
                    Address = @Address,
                    Salary = @Salary,
                    HireDate = @HireDate,
                    Status = @Status,
                    PositionId = @PositionId
                WHERE Id = @Id";

            _logger.LogDebug("SQL: Updating employee with Id {Id}.", id);

            return await _connection.ExecuteAsync(sql, new
            {
                Id = id,
                dto.FirstName,
                dto.Lastname,
                dto.NationalId,
                dto.Email,
                dto.PhoneNumber,
                dto.DateOfBirth,
                dto.Address,
                dto.Salary,
                dto.HireDate,
                dto.Status,
                dto.PositionId
            });
        }

        public async Task<int> SoftDeleteAsync(int id)
        {
            var sql = "UPDATE Employees SET IsDeleted = 1 WHERE Id = @Id";

            _logger.LogDebug("SQL: Soft deleting employee with Id {Id}.", id);

            return await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
