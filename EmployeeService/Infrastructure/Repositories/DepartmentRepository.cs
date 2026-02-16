namespace EmployeeService.Infrastructure.Repositories
{
    public class DepartmentRepository : Repository<Department>
    {
        public DepartmentRepository(
            IDbConnectionFactory connectionFactory,
            ILogger<Repository<Department>> logger)
            : base(connectionFactory, logger)
        {
        }
        protected override string TableName => "Departments";

        public override async Task<int> AddAsync(Department department)
        {
            var sql = $@"
            INSERT INTO {TableName}
                (DepartmentName, ManagerId, IsActive)
            VALUES
                (@DepartmentName, @ManagerId, @IsActive);

            SELECT CAST(SCOPE_IDENTITY() as int);
        ";

            return await _connection.ExecuteScalarAsync<int>(sql, department);
        }

        public override async Task<int> UpdateAsync(int id, Department department)
        {
            var sql = $@"
            UPDATE {TableName}
            SET
                DepartmentName = @DepartmentName,
                ManagerId = @ManagerId,
                IsActive = @IsActive
            WHERE Id = @Id
        ";

            return await _connection.ExecuteAsync(sql, new
            {
                Id = id,
                department.DepartmentName,
                department.ManagerId,
                department.IsActive
            });
        }
    }
}