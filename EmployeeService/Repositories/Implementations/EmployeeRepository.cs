using Dapper;
using EmployeeService.Models;
using EmployeeService.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

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

        public async Task<int> AddAsync(Employee entity)
        {
            var sql = @"INSERT INTO Employees (FirstName, Lastname, NationalId, Email, PhoneNumber, DateOfBirth, Address, Salary, HireDate, Status, DepartmentId, PositionId) 
                        VALUES (@FirstName, @Lastname, @NationalId, @Email, @PhoneNumber, @DateOfBirth, @Address, @Salary, @HireDate, @Status, @DepartmentId, @PositionId)
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";
            
            return await _connection.QuerySingleAsync<int>(sql, entity);
        }
        public async Task<int> UpdateAsync(Employee entity)
        {
            var sql = @"UPDATE Employees SET FirstName = @FirstName, Lastname = @Lastname, NationalId = @NationalId, Email = @Email, PhoneNumber = @PhoneNumber, DateOfBirth = @DateOfBirth, Address = @Address, Salary = @Salary, HireDate = @HireDate, Status = @Status, DepartmentId = @DepartmentId, PositionId = @PositionId
                        WHERE Id = @Id";

            return await _connection.ExecuteAsync(sql, entity);
        }
        public async Task<int> SoftDeleteAsync(int id)
        {
            var sql = "UPDATE Employees SET IsDeleted = 1 WHERE Id = @Id";
            
            return await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
