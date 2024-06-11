using DataLayer;
using Domain.Providers;
using Models;
using Task = System.Threading.Tasks.Task;

namespace Domain.Services
{
  public class TeamService : ITeamService
  {
    private readonly ITeamRepository repository;
    private readonly IUserService userService;

    public TeamService(ITeamRepository repository, IUserService userService)
    {
      this.repository = repository;
      this.userService = userService;
    }

    public async Task<Team> GetByIdAsync(Guid id)
      => await repository.GetByIdAsync(id);

    public async Task<PageableResult<Team>> GetAllAsync(PageableInput input)
      => await repository.GetAllAsync(input);

    public async Task<Team> AddAsync(Team team)
    {
      await ValidateTeamName(team.Name);

      team.Id = GuidProvider.GenetareGuid();

      return await repository.StoreAsync(team);
    }

    public async Task DeleteAsync(Guid id)
      => await repository.RemoveAsync(id);

    public async Task<Team> AddTeamLeader(Guid teamId, Guid userId)
    {
      var team = await repository.GetByIdAsync(teamId);
      if (team == null)
      {
        return null;
      }

      await ValidateUserInTeam(teamId, userId);

      team.TeamLeaderId = userId;

      var changes = new Change<Team>
      {
        Data = team,
        Updates = new List<string> { nameof(team.TeamLeaderId) }
      };

      return await Patch(teamId, changes);
    }

    public async Task<Team> AddUserToTeam(Guid teamId, Guid userId)
    {
      var team = await repository.GetByIdAsync(teamId);

      if (team == null)
      {
        return null;
      }

      await ValidateUserInTeam(teamId, userId);

      team.UserIds = (team.UserIds ?? new List<Guid>())
          .Append(userId)
          .Distinct()
          .OrderBy(s => s)
          .ToList();

      var changes = new Change<Team>
      {
        Data = team,
        Updates = new List<string> { nameof(team.UserIds) }
      };

      return await Patch(teamId, changes);
    }

    public async Task<Team> DeleteUserFromTeam(Guid teamId, Guid userId)
    {
      var team = await repository.GetByIdAsync(teamId);

      if (team == null)
      {
        return null;
      }

      team.UserIds = (team.UserIds ?? new List<Guid>())
        .Where(s => s != userId)
        .OrderBy(s => s)
        .ToList();

      var changes = new Change<Team>
      {
        Data = team,
        Updates = new List<string> { nameof(team.UserIds) }
      };

      return await Patch(teamId, changes);
    }

    public async Task<Team> Patch(Guid id, Change<Team> team)
    {
      if (team.Updates.Contains(nameof(Team.Name)))
      {
        await ValidateTeamName(team.Data.Name);
      }
      return await repository.ChangeOneAsync(id, team);
    }

    private async Task ValidateUserInTeam(Guid teamId, Guid userId)
    {
      var exisitng = await userService.GetByIdAsync(userId);

      if (exisitng == null)
      {
        throw new InvalidDataException($"Nie znaleziono użytkownika o Id: {userId}");
      }

      var input = new PageableInput { PageNumber = 0, PageSize = int.MaxValue };
      var allTeams = await repository.GetAllAsync(input);
      var filteredTeams = allTeams.Items.Where(s => s.Id != teamId);

      if (filteredTeams.Any(s => s.UserIds?.Contains(userId) == true) || filteredTeams.Any(s => s.TeamLeaderId == userId))
      {
        throw new InvalidDataException("Użytkownik jest już przypisany do innego zespołu !");
      }
    }

    private async Task ValidateTeamName(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new InvalidDataException("Nie podano nazwy zespołu !");
      }

      var input = new PageableInput() { PageNumber = 0, PageSize = int.MaxValue };
      var allTeams = await repository.GetAllAsync(input);

      if (allTeams.Items.Any(s => s.Name == name))
      {
        throw new InvalidDataException($"Istnieje już zespół o nazwie {name} !");
      }
    }
  }
}
