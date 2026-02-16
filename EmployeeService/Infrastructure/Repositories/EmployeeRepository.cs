namespace EmployeeService.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>
    {
        public EmployeeRepository(
            IDbConnectionFactory connectionFactory,
            ILogger<Repository<Employee>> logger)
            : base(connectionFactory, logger)
        {
        }

        protected override string TableName => "Employees";

        public override async Task<int> AddAsync(Employee employee)
        {
            var sql = @"
                INSERT INTO Employees
                (
                    FirstName,
                    Lastname,
                    NationalId,
                    Email,
                    PhoneNumber,
                    DateOfBirth,
                    Address,
                    Salary,
                    HireDate,
                    Status,
                    PositionId,
                    IsDeleted
                )
                VALUES
                (
                    @FirstName,
                    @Lastname,
                    @NationalId,
                    @Email,
                    @PhoneNumber,
                    @DateOfBirth,
                    @Address,
                    @Salary,
                    @HireDate,
                    @Status,
                    @PositionId,
                    @IsDeleted
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            return await _connection.ExecuteScalarAsync<int>(sql, employee);
        }

        public override async Task<int> UpdateAsync(int id, Employee employee)
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
                employee.FirstName,
                employee.Lastname,
                employee.NationalId,
                employee.Email,
                employee.PhoneNumber,
                employee.DateOfBirth,
                employee.Address,
                employee.Salary,
                employee.HireDate,
                employee.Status,
                employee.PositionId
            });
        }
    }
}
