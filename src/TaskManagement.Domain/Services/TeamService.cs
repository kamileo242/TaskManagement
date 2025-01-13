using DataLayer;
using Models;
using TaskManagement.Models.Exceptions;
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
    {
      var team = await repository.GetByIdAsync(id);

      if (team == null)
      {
        throw new MissingDataException($"Nie znaleziono zespołu o id: {id}.");
      }

      return team;
    }

    public async Task<PageableResult<Team>> GetAllAsync(PageableInput input)
      => await repository.GetAllAsync(input);

    public async Task<Team> AddAsync(IHistoryUpdater updater, Team team)
    {
      await ValidateTeamName(team.Name);

      var result = await repository.StoreAsync(team);

      updater.SetObjectId<Team>(result.Id);
      updater.SetChangeDetails(null, result);

      return result;
    }

    public async Task DeleteAsync(IHistoryUpdater updater, Guid id)
    {
      updater.SetObjectId<Team>(id);

      await repository.RemoveAsync(id);
    }

    public async Task<Team> AddTeamLeader(IHistoryUpdater updater, Guid teamId, Guid userId)
    {
      var team = await repository.GetByIdAsync(teamId);

      if (team == null)
      {
        throw new MissingDataException($"Nie znaleziono zespołu o id: {teamId}.");
      }

      var originalTeam = new Team
      {
        Id = team.Id,
        Name = team.Name,
        TeamLeaderId = team.TeamLeaderId,
        UserIds = team.UserIds
      };

      await ValidateUserInTeam(teamId, userId);

      team.TeamLeaderId = userId;

      var changes = new Change<Team>
      {
        Data = team,
        Updates = new List<string> { nameof(team.TeamLeaderId) }
      };

      var result = await Patch(teamId, changes);

      updater.SetObjectId<Team>(result.Id);
      updater.SetChangeDetails(originalTeam, result);

      return result;
    }


    public async Task<Team> AddUserToTeam(IHistoryUpdater updater, Guid teamId, Guid userId)
    {
      var team = await repository.GetByIdAsync(teamId);

      if (team == null)
      {
        throw new MissingDataException($"Nie znaleziono zespołu o id: {teamId}.");
      }

      var originalTeam = new Team
      {
        Id = team.Id,
        Name = team.Name,
        TeamLeaderId = team.TeamLeaderId,
        UserIds = team.UserIds
      };

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

      var result = await Patch(teamId, changes);

      updater.SetObjectId<Team>(result.Id);
      updater.SetChangeDetailsList(originalTeam, result);

      return result;
    }

    public async Task<Team> DeleteUserFromTeam(IHistoryUpdater updater, Guid teamId, Guid userId)
    {
      var team = await repository.GetByIdAsync(teamId);

      if (team == null)
      {
        throw new MissingDataException($"Nie znaleziono zespołu o id: {teamId}.");
      }

      var originalTeam = new Team
      {
        Id = team.Id,
        Name = team.Name,
        TeamLeaderId = team.TeamLeaderId,
        UserIds = team.UserIds
      };

      team.UserIds = (team.UserIds ?? new List<Guid>())
        .Where(s => s != userId)
        .OrderBy(s => s)
        .ToList();

      var changes = new Change<Team>
      {
        Data = team,
        Updates = new List<string> { nameof(team.UserIds) }
      };

      var result = await Patch(teamId, changes);

      updater.SetObjectId<Team>(result.Id);
      updater.SetChangeDetailsList(originalTeam, result);

      return result;
    }

    public async Task<Team> PatchAsync(IHistoryUpdater updater, Guid id, Change<Team> team)
    {
      var existingTeam = await repository.GetByIdAsync(id);

      if (existingTeam == null)
      {
        throw new MissingDataException($"Nie znaleziono zespołu o id: {id}.");
      }

      var result = await Patch(id, team);

      updater.SetObjectId<Team>(result.Id);
      updater.SetChangeDetails(existingTeam, result);

      return result;
    }

    private async Task<Team> Patch(Guid id, Change<Team> team)
    {
      if (team.Updates.Contains(nameof(Team.Name), StringComparer.InvariantCultureIgnoreCase))
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
        throw new MissingDataException($"Nie znaleziono użytkownika o Id: {userId}.");
      }

      var input = new PageableInput { PageNumber = 0, PageSize = int.MaxValue };
      var allTeams = await repository.GetAllAsync(input);
      var filteredTeams = allTeams.Items.Where(s => s.Id != teamId);

      if (filteredTeams.Any(s => s.UserIds?.Contains(userId) == true) || filteredTeams.Any(s => s.TeamLeaderId == userId))
      {
        throw new IncorrectDataException("Użytkownik jest już przypisany do innego zespołu.");
      }
    }

    private async Task ValidateTeamName(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new IncorrectDataException("Nie podano nazwy zespołu.");
      }

      var input = new PageableInput() { PageNumber = 0, PageSize = int.MaxValue };
      var allTeams = await repository.GetAllAsync(input);

      if (allTeams.Items.Any(s => s.Name == name))
      {
        throw new IncorrectDataException($"Istnieje już zespół o nazwie {name}.");
      }
    }
  }
}
