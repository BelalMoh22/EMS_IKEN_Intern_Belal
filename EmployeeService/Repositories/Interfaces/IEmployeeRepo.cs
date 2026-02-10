namespace EmployeeService.Repositories.Interfaces
{
    public interface IEmployeeRepo
    {
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
        Task<Employee> GetByIdAsync(int id);
        Task<int> AddAsync(CreateEmployeeDTO entity);
        Task<int> UpdateAsync(int id, UpdateEmployeeDTO entity);
        Task<int> SoftDeleteAsync(int id);
    }
}
