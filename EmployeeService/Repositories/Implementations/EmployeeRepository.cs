using Dapper;

namespace EmployeeService.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepo
    {
        private readonly IDbConnection _connection;
        public EmployeeRepository(IConfiguration configuration)
        { 
            _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
  
        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        {
            var sql = "SELECT * FROM Employees WHERE IsDeleted = 0";
            var employees = await _connection.QueryAsync<Employee>(sql);
            return employees;
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Employees WHERE Id =@Id";
            var employee = await _connection.QueryFirstOrDefaultAsync<Employee>(sql , new { Id = id });
            return employee;
        }

        public async Task<int> AddAsync(CreateEmployeeDTO emp)
        {
            var sql = @"INSERT INTO Employees (FirstName, Lastname, NationalId, Email, PhoneNumber, DateOfBirth, Address, Salary, HireDate, Status, PositionId) 
                        VALUES (@FirstName, @Lastname, @NationalId, @Email, @PhoneNumber, @DateOfBirth, @Address, @Salary, @HireDate, @Status, @PositionId)
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await _connection.QuerySingleAsync<int>(sql, emp);
        }
        public async Task<int> UpdateAsync(int id, UpdateEmployeeDTO entity)
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
                        WHERE Id = @Id
                    ";

            return await _connection.ExecuteAsync(sql, new
            {
                Id = id,
                entity.FirstName,
                entity.Lastname,
                entity.NationalId,
                entity.Email,
                entity.PhoneNumber,
                entity.DateOfBirth,
                entity.Address,
                entity.Salary,
                entity.HireDate,
                entity.Status,
                entity.PositionId
            });
        }
        public async Task<int> SoftDeleteAsync(int id)
        {
            var sql = "UPDATE Employees SET IsDeleted = 1 WHERE Id = @Id";
            
            return await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
