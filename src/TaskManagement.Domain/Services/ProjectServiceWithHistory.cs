using Models;
using Task = System.Threading.Tasks.Task;

namespace Domain.Services
{
  public class ProjectServiceWithHistory : IProjectServiceWithHistory
  {
    private readonly IProjectService projectService;
    private readonly IExecuteWithHistoryService executeWithHistoryService;

    public ProjectServiceWithHistory(IProjectService projectService, IExecuteWithHistoryService executeWithHistoryService)
    {
      this.projectService = projectService;
      this.executeWithHistoryService = executeWithHistoryService;
    }
    public async Task<Project> GetByIdAsync(Guid id)
      => await projectService.GetByIdAsync(id);

    public async Task<PageableResult<Project>> GetAllAsync(PageableInput input)
      => await projectService.GetAllAsync(input);

    public async Task<Project> AddAsync(OperationContext context, Project project)
      => await executeWithHistoryService.Execute(
        action => projectService.AddAsync(action, project),
        context);

    public async Task<Project> AddCommentAsync(OperationContext context, Guid projectId, Comment comment)
      => await executeWithHistoryService.Execute(
        action => projectService.AddCommentAsync(action, projectId, comment),
        context);

    public async Task<Project> DeleteCommentAsync(OperationContext context, Guid projectId, Guid commentId)
      => await executeWithHistoryService.Execute(
        action => projectService.DeleteCommentAsync(action, projectId, commentId),
        context);

    public async Task<Project> DeleteTaskAsync(OperationContext context, Guid projectId, Guid taskId)
      => await executeWithHistoryService.Execute(
        action => projectService.DeleteTaskAsync(action, projectId, taskId),
        context);

    public async Task<Project> EndProjectAsync(OperationContext context, Guid projectId)
      => await executeWithHistoryService.Execute(
        action => projectService.EndProjectAsync(action, projectId),
        context);

    public async Task<Project> PatchAsync(OperationContext context, Guid id, Change<Project> project)
      => await executeWithHistoryService.Execute(
        action => projectService.PatchAsync(action, id, project),
        context);

    public async Task DeleteAsync(OperationContext context, Guid id)
    {
      await executeWithHistoryService.Execute(
          async action =>
          {
            await projectService.DeleteAsync(action, id);
            return Task.CompletedTask;
          },
          context);
    }
  }
}
