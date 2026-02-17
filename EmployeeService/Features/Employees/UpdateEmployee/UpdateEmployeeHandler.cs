namespace EmployeeService.Features.Employees.Handlers.Implementations
{
    public class UpdateEmployeeHandler: IRequestHandler<UpdateEmployeeCommand, int>
    {
        private readonly IRepository<Employee> _repo;
        private readonly IEmployeeBusinessRules _rules;

        public UpdateEmployeeHandler(IRepository<Employee> repo,IEmployeeBusinessRules rules)
        {
            _repo = repo;
            _rules = rules;
        }

        public async Task<int> Handle(UpdateEmployeeCommand request,CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new Exceptions.ValidationException(new() { "Invalid employee Id." });

            var existingEmployee = await _repo.GetByIdAsync(request.Id);
            if (existingEmployee is null)
                throw new NotFoundException($"existingEmployee with Id {request.Id} not found.");

            var dto = request.dto;
            ValidationHelper.ValidateModel(dto);
            await _rules.ValidateAsync(request.Id, existingEmployee.Email, existingEmployee.NationalId, existingEmployee.Salary, existingEmployee.PositionId);

            existingEmployee.FirstName = dto.FirstName ?? existingEmployee.FirstName;
            existingEmployee.Lastname = dto.Lastname ?? existingEmployee.Lastname;
            existingEmployee.NationalId = dto.NationalId ?? existingEmployee.NationalId;
            existingEmployee.Email = dto.Email ?? existingEmployee.Email;
            existingEmployee.PhoneNumber = dto.PhoneNumber ?? existingEmployee.PhoneNumber;
            existingEmployee.DateOfBirth = dto.DateOfBirth ?? existingEmployee.DateOfBirth;
            existingEmployee.Address = dto.Address ?? existingEmployee.Address;
            existingEmployee.Salary = dto.Salary ?? existingEmployee.Salary;
            existingEmployee.HireDate = dto.HireDate ?? existingEmployee.HireDate;
            existingEmployee.Status = dto.Status ?? existingEmployee.Status;
            existingEmployee.PositionId = dto.PositionId ?? existingEmployee.PositionId;

            return await _repo.UpdateAsync(request.Id, existingEmployee);
        }
    }
}
