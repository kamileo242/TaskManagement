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
      var expectedTeam = new Team
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Name = "NET",
      };
      mockRepository.Setup(s => s.GetByIdAsync(expectedTeam.Id)).ReturnsAsync(expectedTeam);

      var result = await teamService.GetByIdAsync(expectedTeam.Id);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expectedTeam);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_null_when_team_not_found()
    {
      var nonExistentteamId = Guid.Parse("00000000000000000000000000000001");
      mockRepository.Setup(s => s.GetByIdAsync(nonExistentteamId)).ReturnsAsync((Team) null);

      var result = await teamService.GetByIdAsync(nonExistentteamId);

      result.Should().BeNull();
    }

    [Test]
    public async Task GetByIdAsync_Should_return_teams_with_pagination()
    {
      var input = new PageableInput
      {
        PageNumber = 0,
        PageSize = 50,
      };
      var expectedTeams = new PageableResult<Team>
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
      mockRepository.Setup(s => s.GetAllAsync(input)).ReturnsAsync(expectedTeams);

      var result = await teamService.GetAllAsync(input);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expectedTeams);
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
      var expectedAllTeams = new PageableResult<Team>
      {
        Items = new Team[] { new Team { Id = Guid.Parse("00000000000000000000000000000002"), Name = "JAVA" } },
        Pagination = new Pagination { PageNumber = 0, PageSize = int.MaxValue, TotalElements = 1 }
      };
      mockRepository.Setup(s => s.GetAllAsync(input)).ReturnsAsync(expectedAllTeams);
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
  }
}
