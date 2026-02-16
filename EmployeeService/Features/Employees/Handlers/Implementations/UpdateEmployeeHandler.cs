namespace EmployeeService.Features.Employees.Handlers.Implementations
{
    public class UpdateEmployeeHandler : IUpdateEmployeeHandler
    {
        private readonly IRepository<Employee> _repo;

        public UpdateEmployeeHandler(IRepository<Employee> repo)
        {
            _repo = repo;
        }

        public async Task<int> HandleAsync(int id, UpdateEmployeeDTO dto)
        {
            if (id <= 0)
                throw new Exceptions.ValidationException("Invalid employee Id.");

            var existingEmployee = await _repo.GetByIdAsync(id);

            if (existingEmployee == null)
                throw new NotFoundException($"Employee with Id {id} not found.");

            // Map DTO → Existing Entity
            existingEmployee.FirstName = dto.FirstName;
            existingEmployee.Lastname = dto.Lastname;
            existingEmployee.NationalId = dto.NationalId;
            existingEmployee.Email = dto.Email;
            existingEmployee.PhoneNumber = dto.PhoneNumber;
            existingEmployee.DateOfBirth = dto.DateOfBirth;
            existingEmployee.Address = dto.Address;
            existingEmployee.Salary = dto.Salary;
            existingEmployee.HireDate = dto.HireDate;
            existingEmployee.Status = dto.Status;
            existingEmployee.PositionId = dto.PositionId;

            var rows = await _repo.UpdateAsync(id, existingEmployee);

            return rows;
        }

    }
}
