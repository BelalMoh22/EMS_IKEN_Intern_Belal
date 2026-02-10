namespace EmployeeService.Repositories.Implementations
{
    public class PositionRepository : GenericRepo<Position>
    {
        public PositionRepository(IConfiguration configuration) : base(configuration) {}

        public override async Task<int> AddAsync(Position position)
        {
            var sql = @"
                INSERT INTO Positions
                    (PositionName, MinSalary, MaxSalary)
                VALUES
                    (@PositionName, @MinSalary, @MaxSalary);

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            return await _connection.ExecuteScalarAsync<int>(sql, position);
        }

        public override async Task<int> UpdateAsync(int id, Position position)
        {
            var sql = @"
                UPDATE Positions
                SET
                    PositionName = @PositionName,
                    MinSalary = @MinSalary,
                    MaxSalary = @MaxSalary
                WHERE Id = @Id
            ";

            return await _connection.ExecuteAsync(sql, new
            {
                Id = id,
                position.PositionName,
                position.MinSalary,
                position.MaxSalary,
                position.DepartmentId,
            });
        }
    }
}
