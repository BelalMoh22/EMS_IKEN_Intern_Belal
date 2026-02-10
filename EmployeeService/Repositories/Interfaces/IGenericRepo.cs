namespace EmployeeService.Repositories.Interfaces
{
    public interface IGenericRepo<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(int id , T entity);
        Task<int> DeleteAsync(int id);
    }
}
