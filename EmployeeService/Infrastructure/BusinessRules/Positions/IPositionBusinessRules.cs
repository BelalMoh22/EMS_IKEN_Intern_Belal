namespace EmployeeService.Infrastructure.BusinessRules.Positions
{
    public interface IPositionBusinessRules
    {
        //Task ValidateAsync(decimal minSalary, decimal maxSalary, int departmentId);
        Task ValidateForCreateAsync(CreatePositionDto dto);
        Task ValidateForUpdateAsync(int positionId, UpdatePositionDto dto, Position existingPosition);
    }
}
