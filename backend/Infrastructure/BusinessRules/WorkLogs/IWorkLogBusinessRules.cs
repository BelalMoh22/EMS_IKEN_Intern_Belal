namespace backend.Infrastructure.BusinessRules.WorkLogs
{
    public interface IWorkLogBusinessRules
    {
        Task ValidateProjectAsync(int projectId, Dictionary<string, List<string>> errors);
        Task ValidateTimesheetAsync(SaveTimesheetDTO dto);
    }
}
