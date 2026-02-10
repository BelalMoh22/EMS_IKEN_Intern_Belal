using Dapper;
using EmployeeService.Models;

namespace EmployeeService.Repositories.Implementations
{
    public class DepartmentRepository : GenericRepo<Department>
    {
        public DepartmentRepository(IConfiguration configuration) : base(configuration) {}

        public override async Task<int> AddAsync(Department department)
        {
            var sql = @"
                INSERT INTO Departments
                    (DepartmentName, ManagerId, IsActive)
                VALUES
                    (@DepartmentName, @ManagerId, @IsActive)
                SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            return await _connection.ExecuteScalarAsync<int>(sql, department);
        }

        public override async Task<int> UpdateAsync(Department department)
        {
            var sql = @"
                UPDATE Departments
                SET
                    DepartmentName = @DepartmentName,
                    ManagerId = @ManagerId,
                    IsActive = @IsActive
                WHERE Id = @Id
            ";
            return await _connection.ExecuteAsync(sql, department);
        }
    }
}
