using DataLayer;
using Domain;
using Domain.Services;
using FluentAssertions;
using Models;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace ServiceTests
{
  [TestFixture]
  public class TeamServiceTests
  {
    private Mock<ITeamRepository> mockRepository;
    private Mock<IUserService> mockUserService;
    private ITeamService teamService;

    [SetUp]
    public void Setup()
    {
      mockRepository = new Mock<ITeamRepository>();
      mockUserService = new Mock<IUserService>();
      teamService = new TeamService(mockRepository.Object, mockUserService.Object);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_team_when_valid_id()
    {
      var expected = new Team
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Name = "NET",
      };
      mockRepository.Setup(s => s.GetByIdAsync(expected.Id)).ReturnsAsync(expected);

      var result = await teamService.GetByIdAsync(expected.Id);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_null_when_team_not_found()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      mockRepository.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync((Team) null);

      var result = await teamService.GetByIdAsync(teamId);

      result.Should().BeNull();
    }

    [Test]
    public async Task GetAllAsync_Should_return_teams_with_pagination()
    {
      var input = new PageableInput
      {
        PageNumber = 0,
        PageSize = 50,
      };
      var expected = new PageableResult<Team>
      {
        Items = new Team[]
        {
          new Team
          {
            Id = Guid.Parse("00000000000000000000000000000001"),
            Name = "NET",
          }
        },
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 1
        }
      };
      mockRepository.Setup(s => s.GetAllAsync(input)).ReturnsAsync(expected);

      var result = await teamService.GetAllAsync(input);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddAsync_Should_return_stored_team_when_valid_data()
    {
      var input = new PageableInput { PageNumber = 0, PageSize = int.MaxValue };
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var teamToAdd = new Team
      {
        Name = "NET",
      };
      var allTeams = new PageableResult<Team>
      {
        Items = new Team[] { new Team { Id = Guid.Parse("00000000000000000000000000000002"), Name = "JAVA" } },
        Pagination = new Pagination { PageNumber = 0, PageSize = int.MaxValue, TotalElements = 1 }
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(allTeams);
      mockRepository.Setup(s => s.StoreAsync(It.IsAny<Team>()))
                    .ReturnsAsync((Team team) =>
                    {
                      team.Id = teamId;
                      return team;
                    });

      var result = await teamService.AddAsync(teamToAdd);

      result.Should().NotBeNull();
      result.Id.Should().Be("00000000000000000000000000000001");
      result.Should().BeEquivalentTo(teamToAdd, o => o.Excluding(s => s.Id));
      result.Id.Should().Be(teamId);
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_team_with_the_same_name_is_already_exist()
    {
      var input = new PageableInput { PageNumber = 0, PageSize = int.MaxValue };
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var teamToAdd = new Team
      {
        Name = "NET",
      };
      var allTeams = new PageableResult<Team>
      {
        Items = new Team[] { new Team { Id = Guid.Parse("00000000000000000000000000000002"), Name = "NET" } },
        Pagination = new Pagination { PageNumber = 0, PageSize = int.MaxValue, TotalElements = 1 }
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(allTeams);
      mockRepository.Setup(s => s.StoreAsync(It.IsAny<Team>()))
                    .ReturnsAsync((Team team) =>
                    {
                      team.Id = teamId;
                      return team;
                    });

      var action = async () => await teamService.AddAsync(teamToAdd);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage($"Istnieje już zespół o nazwie {teamToAdd.Name}.");
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_store_team_not_contains_name()
    {
      var action = async () => await teamService.AddAsync(new Team());

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Nie podano nazwy zespołu.");
    }

    [Test]
    public async Task DeleteAsync_Should_remove_team()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");

      await teamService.DeleteAsync(teamId);

      mockRepository.Verify(s => s.RemoveAsync(teamId), Times.Once);
    }

    [Test]
    public async Task AddTeamLeaderAsync_Should_return_updated_team_when_valid_data()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      var user = new User
      {
        Id = userId,
        Name = "Tomek",
        Surname = "Solecki",
        Email = "tsolecki@gmail.com",
        Position = "Stażysta",
        PhoneNumber = "1234567890"
      };
      var team = new Team
      {
        Id = teamId,
        Name = "NET"
      };
      var allTeams = new PageableResult<Team>
      {
        Items = new Team[] { team },
        Pagination = new Pagination { PageNumber = 0, PageSize = 50, TotalElements = 1 }
      };
      var expected = new Team
      {
        Id = teamId,
        Name = "NET",
        TeamLeaderId = userId,
      };
      mockRepository.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync(team);
      mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(allTeams);
      mockRepository.Setup(s => s.ChangeOneAsync(teamId, It.IsAny<Change<Team>>())).ReturnsAsync(expected);

      var result = await teamService.AddTeamLeader(teamId, userId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddTeamLeaderAsync_Should_return_null_when_team_is_not_exist()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      mockRepository.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync((Team) null);

      var result = await teamService.AddTeamLeader(teamId, userId);

      result.Should().BeNull();
    }

    [Test]
    public async Task AddTeamLeaderAsync_Should_throw_exception_when_user_is_not_exist()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      var team = new Team
      {
        Id = teamId,
        Name = "NET"
      };
      mockRepository.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync(team);
      mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((User) null);

      var action = async () => await teamService.AddTeamLeader(teamId, userId);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage($"Nie znaleziono użytkownika o Id: {userId}.");
    }

    [Test]
    public async Task AddTeamLeaderAsync_Should_throw_exception_when_user_is_in_other_team()
    {
      var teamId1 = Guid.Parse("00000000000000000000000000000001");
      var teamId2 = Guid.Parse("00000000000000000000000000000002");
      var userId = Guid.Parse("00000000000000000000000000000003");
      var user = new User
      {
        Id = userId,
        Name = "Tomek",
        Surname = "Solecki",
        Email = "tsolecki@gmail.com",
        Position = "Stażysta",
        PhoneNumber = "1234567890"
      };
      var team1 = new Team
      {
        Id = teamId1,
        Name = "NET"
      };
      var team2 = new Team
      {
        Id = teamId2,
        Name = "Java",
        TeamLeaderId = userId,
      };
      var allTeams = new PageableResult<Team>
      {
        Items = new Team[] { team1, team2 },
        Pagination = new Pagination { PageNumber = 0, PageSize = 50, TotalElements = 2 }
      };
      mockRepository.Setup(s => s.GetByIdAsync(teamId1)).ReturnsAsync(team1);
      mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(allTeams);

      var action = async () => await teamService.AddTeamLeader(teamId1, userId);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Użytkownik jest już przypisany do innego zespołu.");
    }

    [Test]
    public async Task AddUserToTeam_Should_return_updated_team_when_valid_data()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      var user = new User
      {
        Id = userId,
        Name = "Tomek",
        Surname = "Solecki",
        Email = "tsolecki@gmail.com",
        Position = "Stażysta",
        PhoneNumber = "1234567890"
      };
      var team = new Team
      {
        Id = teamId,
        Name = "NET"
      };
      var allTeams = new PageableResult<Team>
      {
        Items = new Team[] { team },
        Pagination = new Pagination { PageNumber = 0, PageSize = 50, TotalElements = 1 }
      };
      var expected = new Team
      {
        Id = teamId,
        Name = "NET",
        UserIds = new List<Guid> { userId },
      };
      mockRepository.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync(team);
      mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(allTeams);
      mockRepository.Setup(s => s.ChangeOneAsync(teamId, It.IsAny<Change<Team>>())).ReturnsAsync(expected);

      var result = await teamService.AddUserToTeam(teamId, userId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddUserToTeam_Should_return_the_same_team_when_user_is_already_exist_in_team()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      var user = new User
      {
        Id = userId,
        Name = "Tomek",
        Surname = "Solecki",
        Email = "tsolecki@gmail.com",
        Position = "Stażysta",
        PhoneNumber = "1234567890"
      };
      var team = new Team
      {
        Id = teamId,
        Name = "NET",
        UserIds = new List<Guid> { userId },
      };
      var allTeams = new PageableResult<Team>
      {
        Items = new Team[] { team },
        Pagination = new Pagination { PageNumber = 0, PageSize = 50, TotalElements = 1 }
      };
      var expected = new Team
      {
        Id = teamId,
        Name = "NET",
        UserIds = new List<Guid> { userId },
      };
      mockRepository.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync(team);
      mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(allTeams);
      mockRepository.Setup(s => s.ChangeOneAsync(teamId, It.IsAny<Change<Team>>())).ReturnsAsync(expected);

      var result = await teamService.AddUserToTeam(teamId, userId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddUserToTeam_Should_return_null_when_team_is_not_exist()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      mockRepository.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync((Team) null);

      var result = await teamService.AddUserToTeam(teamId, userId);

      result.Should().BeNull();
    }

    [Test]
    public async Task AddUserToTeam_Should_throw_exception_when_user_is_not_exist()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      var team = new Team
      {
        Id = teamId,
        Name = "NET"
      };
      mockRepository.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync(team);
      mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((User) null);

      var action = async () => await teamService.AddUserToTeam(teamId, userId);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage($"Nie znaleziono użytkownika o Id: {userId}.");
    }

    [Test]
    public async Task AddUserToTeam_Should_throw_exception_when_user_is_in_other_team()
    {
      var teamId1 = Guid.Parse("00000000000000000000000000000001");
      var teamId2 = Guid.Parse("00000000000000000000000000000002");
      var userId = Guid.Parse("00000000000000000000000000000003");
      var user = new User
      {
        Id = userId,
        Name = "Tomek",
        Surname = "Solecki",
        Email = "tsolecki@gmail.com",
        Position = "Stażysta",
        PhoneNumber = "1234567890"
      };
      var team1 = new Team
      {
        Id = teamId1,
        Name = "NET"
      };
      var team2 = new Team
      {
        Id = teamId2,
        Name = "Java",
        TeamLeaderId = userId,
      };
      var allTeams = new PageableResult<Team>
      {
        Items = new Team[] { team1, team2 },
        Pagination = new Pagination { PageNumber = 0, PageSize = 50, TotalElements = 2 }
      };
      mockRepository.Setup(s => s.GetByIdAsync(teamId1)).ReturnsAsync(team1);
      mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(allTeams);

      var action = async () => await teamService.AddUserToTeam(teamId1, userId);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Użytkownik jest już przypisany do innego zespołu.");
    }

    [Test]
    public async Task DeleteUserFromTeam_Should_return_updated_team_valid_data()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      var user = new User
      {
        Id = userId,
        Name = "Tomek",
        Surname = "Solecki",
        Email = "tsolecki@gmail.com",
        Position = "Stażysta",
        PhoneNumber = "1234567890"
      };
      var team = new Team
      {
        Id = teamId,
        Name = "NET",
        UserIds = new List<Guid> { userId },
      };
      var expected = new Team
      {
        Id = teamId,
        Name = "NET",
      };
      mockRepository.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync(team);
      mockRepository.Setup(s => s.ChangeOneAsync(teamId, It.IsAny<Change<Team>>())).ReturnsAsync(expected);

      var result = await teamService.DeleteUserFromTeam(teamId, userId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task DeleteUserFromTeam_Should_return_the_same_team_when_user_is_not_in_team()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      var user = new User
      {
        Id = userId,
        Name = "Tomek",
        Surname = "Solecki",
        Email = "tsolecki@gmail.com",
        Position = "Stażysta",
        PhoneNumber = "1234567890"
      };
      var team = new Team
      {
        Id = teamId,
        Name = "NET",
      };
      var expected = new Team
      {
        Id = teamId,
        Name = "NET",
      };
      mockRepository.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync(team);
      mockRepository.Setup(s => s.ChangeOneAsync(teamId, It.IsAny<Change<Team>>())).ReturnsAsync(expected);

      var result = await teamService.DeleteUserFromTeam(teamId, userId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task DeleteUserFromTeam_Should_return_null_when_team_is_not_exist()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      mockRepository.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync((Team) null);

      var result = await teamService.DeleteUserFromTeam(teamId, userId);

      result.Should().BeNull();
    }

    [Test]
    public async Task Patch_Should_return_null_when_team_is_not_exist()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      var allTeams = new PageableResult<Team>
      {
        Items = Array.Empty<Team>(),
        Pagination = new Pagination { PageNumber = 0, PageSize = 50, TotalElements = 0 }
      };
      var changes = new Change<Team>
      {
        Data = new Team { Name = "Java" },
        Updates = new List<string> { nameof(Team.Name) }
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(allTeams);
      mockRepository.Setup(s => s.ChangeOneAsync(teamId, changes)).ReturnsAsync((Team) null);

      var result = await teamService.Patch(teamId, changes);

      result.Should().BeNull();
    }

    [Test]
    public async Task Patch_Should_throw_exception_when_team_with_the_same_name_is_already_exist()
    {
      var teamId1 = Guid.Parse("00000000000000000000000000000001");
      var teamId2 = Guid.Parse("00000000000000000000000000000002");
      var team1 = new Team
      {
        Id = teamId1,
        Name = "NET"
      };
      var team2 = new Team
      {
        Id = teamId2,
        Name = "Java",
      };
      var allTeams = new PageableResult<Team>
      {
        Items = new Team[] { team1, team2 },
        Pagination = new Pagination { PageNumber = 0, PageSize = 50, TotalElements = 2 }
      };
      var changes = new Change<Team>
      {
        Data = new Team { Name = "Java" },
        Updates = new List<string> { nameof(Team.Name) }
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(allTeams);

      var action = async () => await teamService.Patch(teamId1, changes);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage($"Istnieje już zespół o nazwie {team2.Name}.");
    }

    [Test]
    public async Task Patch_Should_return_updated_team_when_valid_data()
    {
      var teamId = Guid.Parse("00000000000000000000000000000001");
      var team1 = new Team
      {
        Id = teamId,
        Name = "NET"
      };
      var allTeams = new PageableResult<Team>
      {
        Items = new Team[] { team1 },
        Pagination = new Pagination { PageNumber = 0, PageSize = 50, TotalElements = 2 }
      };
      var changes = new Change<Team>
      {
        Data = new Team { Name = "Java" },
        Updates = new List<string> { nameof(Team.Name) }
      };
      var expected = new Team
      {
        Id = teamId,
        Name = "Java"
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(allTeams);
      mockRepository.Setup(s => s.ChangeOneAsync(teamId, changes)).ReturnsAsync(expected);

      var result = await teamService.Patch(teamId, changes);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }
  }
}
