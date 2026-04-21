namespace backend.Infrastructure.BusinessRules.WorkLogs
{
    public class WorkLogBusinessRules : BaseBusinessRules, IWorkLogBusinessRules
    {
        private readonly IProjectRepository _projectRepo;

        public WorkLogBusinessRules(IProjectRepository projectRepo)
        {
            _projectRepo = projectRepo;
        }

        // =========================
        // PROJECT VALIDATION
        // =========================
        public async Task ValidateProjectAsync(int projectId, Dictionary<string, List<string>> errors)
        {
            var project = await _projectRepo.GetByIdAsync(projectId);

            if (project == null || project.IsDeleted)
            {
                AddError(errors, "projectId", "Project does not exist.");
                return;
            }

            if (project.Status != ProjectStatus.Open)
                AddError(errors, "projectId", "Cannot log hours on closed project.");
        }

        public async Task ValidateTimesheetAsync(SaveTimesheetDTO dto)
        {
            var errors = new Dictionary<string, List<string>>();

            if (dto.Entries == null || !dto.Entries.Any())
            {
                AddError(errors, "entries", "No entries to save.");
                ThrowIfAny(errors);
            }

            foreach (var entry in dto.Entries)
            {
                if (entry.Hours < 0 || entry.Hours > 24)
                {
                    AddError(errors, "hours", $"Hours must be between 0 and 24. (Project ID: {entry.ProjectId}, Date: {entry.Date})");
                }

                if (!DateTime.TryParse(entry.Date, out _))
                {
                    AddError(errors, "date", $"Invalid date format: {entry.Date}");
                }

                var project = await _projectRepo.GetByIdAsync(entry.ProjectId);
                if (project == null || project.IsDeleted)
                {
                    AddError(errors, "projectId", $"Project {entry.ProjectId} does not exist.");
                }
                else if (project.Status != ProjectStatus.Open)
                {
                    AddError(errors, "projectId", $"Cannot log hours on completed project '{project.Name}'.");
                }
            }

            ThrowIfAny(errors);
        }
    }
}
