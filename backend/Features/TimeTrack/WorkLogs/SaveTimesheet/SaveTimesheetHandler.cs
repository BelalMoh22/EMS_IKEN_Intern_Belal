namespace backend.Features.TimeTrack.WorkLogs.SaveTimesheet
{
    public class SaveTimesheetHandler : IRequestHandler<SaveTimesheetCommand, bool>
    {
        private readonly IWorkLogRepository _repo;
        private readonly EmployeeRepository _employeeRepo;
        private readonly IWorkLogBusinessRules _rules;
        private readonly ICurrentUserService _currentUser;

        public SaveTimesheetHandler(
            IWorkLogRepository repo,
            EmployeeRepository employeeRepo,
            IWorkLogBusinessRules rules,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _employeeRepo = employeeRepo;
            _rules = rules;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(SaveTimesheetCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepo.GetByUserIdAsync(_currentUser.UserId);

            if (employee == null)
                throw new Exception("Employee not found.");

            var dto = request.Dto;

            // Validate using business rules
            await _rules.ValidateTimesheetAsync(dto);

            // Build entities
            var entities = dto.Entries.Select(e => new WorkLog(
                employee.Id,
                e.ProjectId,
                DateTime.Parse(e.Date),
                e.Hours,
                e.Notes
            ));

            await _repo.UpsertTimesheetAsync(employee.Id, entities);

            return true;
        }
    }
}
