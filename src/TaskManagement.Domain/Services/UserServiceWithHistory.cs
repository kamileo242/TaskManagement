using Models;
using Task = System.Threading.Tasks.Task;

namespace Domain.Services
{
  public class UserServiceWithHistory : IUserServiceWithHistory
  {
    private readonly IUserService userService;
    private readonly IExecuteWithHistoryService executeWithHistoryService;

    public UserServiceWithHistory(IUserService userService, IExecuteWithHistoryService executeWithHistoryService)
    {
      this.userService = userService;
      this.executeWithHistoryService = executeWithHistoryService;
    }
    public async Task<User> GetByIdAsync(Guid id)
      => await userService.GetByIdAsync(id);

    public async Task<PageableResult<User>> GetAllAsync(PageableInput input)
      => await userService.GetAllAsync(input);

    public async Task<User> AddAsync(OperationContext context, User user)
      => await executeWithHistoryService.Execute(
        action => userService.AddAsync(action, user),
        context);

    public async Task<User> PatchAsync(OperationContext context, Guid id, Change<User> user)
      => await executeWithHistoryService.Execute(
        action => userService.PatchAsync(action, id, user),
        context);

    public async Task DeleteAsync(OperationContext context, Guid id)
    {
      await executeWithHistoryService.Execute(
          async action =>
          {
            await userService.DeleteAsync(action, id);
            return Task.CompletedTask;
          },
          context);
    }
  }
}
