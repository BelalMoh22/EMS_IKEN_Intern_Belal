namespace EmployeeService.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDbConnection _connection;

        public RefreshTokenRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> AddAsync(RefreshToken token)
        {
            var sql = @"
                INSERT INTO RefreshTokens
                (UserId, Token, Expires, IsRevoked, ReplacedByTokenHash)
                VALUES
                (@UserId, @Token, @Expires, @IsRevoked, @ReplacedByTokenHash);

                SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            return await _connection.ExecuteScalarAsync<int>(sql, token);
        }

        public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(int userId)
        {
            var sql = @"
                SELECT * FROM RefreshTokens
                WHERE UserId = @UserId
                ORDER BY CreatedAt DESC
            ";

            return await _connection.QueryAsync<RefreshToken>(
                sql, new { UserId = userId });
        }

        public async Task RevokeAsync(int id, string? replacedByTokenHash = null)
        {
            var sql = @"
                UPDATE RefreshTokens
                SET IsRevoked = 1,
                    ReplacedByTokenHash = @ReplacedByTokenHash
                WHERE Id = @Id
            ";

            await _connection.ExecuteAsync(sql, new
            {
                Id = id,
                ReplacedByTokenHash = replacedByTokenHash
            });
        }

        public async Task DeleteExpiredAsync()
        {
            var sql = @"DELETE FROM RefreshTokens WHERE Expires <= @Now";

            await _connection.ExecuteAsync(sql, new
            {
                Now = DateTime.UtcNow
            });
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            var sql = @"
                SELECT * FROM RefreshTokens
                WHERE Token = @Token
            ";

            return await _connection
                .QueryFirstOrDefaultAsync<RefreshToken>(sql, new { Token = token });
        }
    }
}