namespace EmployeeService.Repositories.Implementations
{
    public abstract class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
    {
        protected readonly IDbConnection _connection;
        protected string _tableName;

        protected GenericRepo(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            _tableName = typeof(T).Name + "s"; // Assuming table names are plural
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {_tableName}";
            var entities = await _connection.QueryAsync<T>(sql);
            return entities;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            var entity = await _connection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
            return entity;
        }

        public abstract Task<int> AddAsync(T entity);

        public abstract Task<int> UpdateAsync(int id ,T entity);

        public async Task<int> DeleteAsync(int id)
        {
            var sql = $"DELETE FROM {_tableName} WHERE Id = @Id";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected;
        }

    }
}
