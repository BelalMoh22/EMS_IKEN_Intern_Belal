namespace EmployeeService.Infrastructure.BusinessRules.Departments
{
    public interface IDepartmentBusinessRules
    {
        //Task ValidateAsync(int managerId);
        Task ValidateForCreateAsync(CreateDepartmentDto dto);
        Task ValidateForUpdateAsync(int departmentId, UpdateDepartmentDto dto, Department existingDepartment);
    }
}
