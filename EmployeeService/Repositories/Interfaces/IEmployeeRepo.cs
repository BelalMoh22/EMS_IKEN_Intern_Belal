using EmployeeService.Models;

namespace EmployeeService.Repositories.Interfaces
{
    public interface IEmployeeRepo
    {
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
        Task<Employee> GetByIdAsync(int id);
        Task<int> AddAsync(Employee entity);
        Task<int> UpdateAsync(Employee entity);
        Task<int> SoftDeleteAsync(int id);
    }
}
