using Models;
using Task = System.Threading.Tasks.Task;

namespace Domain.Services
{
  public class TeamServiceWithHistory : ITeamServiceWithHistory
  {
    private readonly ITeamService teamService;
    private readonly IExecuteWithHistoryService executeWithHistoryService;

    public TeamServiceWithHistory(ITeamService teamService, IExecuteWithHistoryService executeWithHistoryService)
    {
      this.teamService = teamService;
      this.executeWithHistoryService = executeWithHistoryService;
    }

    public async Task<Team> GetByIdAsync(Guid id)
      => await teamService.GetByIdAsync(id);

    public async Task<PageableResult<Team>> GetAllAsync(PageableInput input)
      => await teamService.GetAllAsync(input);

    public async Task<Team> AddAsync(OperationContext context, Team team)
      => await executeWithHistoryService.Execute(
        action => teamService.AddAsync(action, team),
        context);

    public async Task<Team> AddTeamLeader(OperationContext context, Guid teamId, Guid userId)
      => await executeWithHistoryService.Execute(
        action => teamService.AddTeamLeader(action, teamId, userId),
        context);

    public async Task<Team> AddUserToTeam(OperationContext context, Guid teamId, Guid userId)
      => await executeWithHistoryService.Execute(
        action => teamService.AddUserToTeam(action, teamId, userId),
        context);

    public async Task<Team> DeleteUserFromTeam(OperationContext context, Guid teamId, Guid userId)
      => await executeWithHistoryService.Execute(
        action => teamService.DeleteUserFromTeam(action, teamId, userId),
        context);

    public async Task<Team> PatchAsync(OperationContext context, Guid id, Change<Team> team)
    => await executeWithHistoryService.Execute(
        action => teamService.PatchAsync(action, id, team),
        context);

    public async Task DeleteAsync(OperationContext context, Guid id)
    {
      await executeWithHistoryService.Execute(
          async action =>
          {
            await teamService.DeleteAsync(action, id);
            return Task.CompletedTask;
          },
          context);
    }
  }
}
