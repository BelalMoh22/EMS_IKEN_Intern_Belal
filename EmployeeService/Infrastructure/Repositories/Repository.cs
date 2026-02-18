namespace EmployeeService.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly IDbConnectionFactory _connectionFactory;
        protected readonly IDbConnection _connection;
        private readonly ILogger<Repository<T>> _logger;

        protected Repository(
            IDbConnectionFactory connectionFactory,
            ILogger<Repository<T>> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _connection = _connectionFactory.CreateConnection();
        }
        protected abstract string TableName { get; }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {TableName} WHERE IsDeleted = 0";

            _logger.LogDebug("Executing SQL: {Sql}", sql);

            return await _connection.QueryAsync<T>(sql);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE Id = @Id";

            _logger.LogDebug("Executing SQL: {Sql} with Id {Id}", sql, id);

            return await _connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public async Task<int> SoftDeleteAsync(int id)
        {
            var sql = $"UPDATE {TableName} SET IsDeleted = 1 WHERE Id = @Id";

            _logger.LogDebug("Executing SOFT DELETE for Id {Id}", id);

            return await _connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await SoftDeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(string condition, object? parameters = null)
        {
            var sql = $"SELECT 1 FROM {TableName} WHERE {condition}";
            var result = await _connection.QueryFirstOrDefaultAsync<int?>(sql, parameters);
            return result.HasValue;
        }

        public abstract Task<int> AddAsync(T entity);
        public abstract Task<int> UpdateAsync(int id, T entity);
    }
}