using Models;

namespace Domain.Services
{
  public class TaskServiceWithHistory : ITaskServiceWithHistory
  {
    private readonly ITaskService taskService;
    private readonly IExecuteWithHistoryService executeWithHistoryService;

    public TaskServiceWithHistory(ITaskService taskService, IExecuteWithHistoryService executeWithHistoryService)
    {
      this.taskService = taskService;
      this.executeWithHistoryService = executeWithHistoryService;
    }

    public async Task<Models.Task> GetByIdAsync(Guid id)
      => await taskService.GetByIdAsync(id);

    public async Task<PageableResult<Models.Task>> GetAllAsync(PageableInput input)
      => await taskService.GetAllAsync(input);

    public async Task<Models.Task> AddAsync(OperationContext context, Guid projectId, Models.Task task)
      => await executeWithHistoryService.Execute(
        action => taskService.AddAsync(action, projectId, task),
        context);

    public async Task<Models.Task> RegisterTimeAsync(OperationContext context, Guid taskId, int timeInMinutes)
    => await executeWithHistoryService.Execute(
      action => taskService.RegisterTimeAsync(action, taskId, timeInMinutes),
      context);

    public async Task<Models.Task> AssignPersonToTaskAsync(OperationContext context, Guid taskId, Guid userId)
    => await executeWithHistoryService.Execute(
      action => taskService.AssignPersonToTaskAsync(action, taskId, userId),
      context);

    public async Task<Models.Task> AddCommentAsync(OperationContext context, Guid taskId, Comment comment)
    => await executeWithHistoryService.Execute(
      action => taskService.AddCommentAsync(action, taskId, comment),
      context);

    public async Task<Models.Task> DeleteCommentAsync(OperationContext context, Guid taskId, Guid commentId)
    => await executeWithHistoryService.Execute(
      action => taskService.DeleteCommentAsync(action, taskId, commentId),
      context);

    public async Task<Models.Task> EndTaskStatusAsync(OperationContext context, Guid taskId)
    => await executeWithHistoryService.Execute(
      action => taskService.EndTaskStatusAsync(action, taskId),
      context);

    public async Task<Models.Task> PatchAsync(OperationContext context, Guid id, Change<Models.Task> task)
      => await executeWithHistoryService.Execute(
      action => taskService.PatchAsync(action, id, task),
      context);

    public async System.Threading.Tasks.Task DeleteAsync(OperationContext context, Guid id)
    {
      await executeWithHistoryService.Execute(
          async action =>
          {
            await taskService.DeleteAsync(action, id);
            return System.Threading.Tasks.Task.CompletedTask;
          },
          context);
    }
  }
}
