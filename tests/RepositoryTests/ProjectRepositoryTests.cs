using DataLayer;
using DataLayer.Converts;
using DataLayer.Dbos;
using DataLayer.Repositories;
using FluentAssertions;
using Models;
using Models.Statueses;
using Mongo2Go;
using MongoDB.Driver;
using Task = System.Threading.Tasks.Task;

namespace RepositoryTests
{
  [TestFixture]
  public class ProjectRepositoryTests
  {
    private MongoDbRunner runner;
    private IMongoClient client;
    private IMongoDatabase database;
    private ProjectRepository repository;
    private IDboConverter dboConverter;
    private IMongoCollection<ProjectDbo> collection;

    [SetUp]
    public void SetUp()
    {
      runner = MongoDbRunner.Start();
      client = new MongoClient(runner.ConnectionString);
      database = client.GetDatabase("TestDatabase");
      var setup = new DatabaseSetup { ConnectionString = runner.ConnectionString, DatabaseName = "TestDatabase" };

      dboConverter = new DboConverter();
      repository = new ProjectRepository(setup, dboConverter);

      collection = database.GetCollection<ProjectDbo>("projects");
    }

    [TearDown]
    public void TearDown()
    {
      runner.Dispose();
    }

    [Test]
    public async Task StoreAsync_Should_return_new_project()
    {
      var project = new Project
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Title = "Tytuł projektu testowego",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Parse("2024-06-01"),
        Status = ProjectStatus.NotStarted,
        CreatedAt = DateTime.Parse("2024-05-01"),
      };

      var result = await repository.StoreAsync(project);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(project, o => o
        .Excluding(s => s.Comments));
      result.Comments.Should().BeEmpty();
      var retrievedDbo = await DatabaseHelper.GetElementFromCollection(collection, project.Id);
      var retrievedProject = dboConverter.Convert<Project>(retrievedDbo);
      retrievedProject.Should().NotBeNull();
      retrievedProject.Should().BeEquivalentTo(project, o => o
        .Excluding(s => s.Comments));
      retrievedProject.Comments.Should().BeEmpty();
    }

    [Test]
    public async Task GetByIdAsync_Should_return_project_when_project_is_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var project = new ProjectDbo
      {
        Id = projectId,
        Title = "Tytuł projektu testowego",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Parse("2024-06-01"),
        Status = "nierozpoczęty",
        CreatedAt = DateTime.Parse("2024-05-01"),
      };
      var expected = new Project
      {
        Id = projectId,
        Title = "Tytuł projektu testowego",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Parse("2024-06-01"),
        Status = ProjectStatus.NotStarted,
        CreatedAt = DateTime.Parse("2024-05-01"),
        Comments = new List<Comment>() { }
      };
      await DatabaseHelper.AddElementToCollection(collection, project);

      var result = await repository.GetByIdAsync(projectId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
      var retrievedDbo = await DatabaseHelper.GetElementFromCollection(collection, projectId);
      var retrievedProject = dboConverter.Convert<Project>(retrievedDbo);
      retrievedProject.Should().NotBeNull();
      retrievedProject.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_null_when_project_does_not_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");

      var result = await repository.GetByIdAsync(projectId);

      result.Should().BeNull();

      var retrievedDbo = await DatabaseHelper.GetElementFromCollection(collection, projectId);
      var retrievedProject = dboConverter.Convert<Project>(retrievedDbo);
      retrievedProject.Should().BeNull();
    }

    [Test]
    public async Task GetAllAsync_Should_return_projects_with_pagination()
    {
      var project1 = new ProjectDbo
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Title = "Tytuł projektu testowego 1",
        Description = "Opis projektu testowego 1",
        Deadline = DateTime.Parse("2024-06-01"),
        Status = "nierozpoczęty",
        CreatedAt = DateTime.Parse("2024-05-01"),
      };
      var project2 = new ProjectDbo
      {
        Id = Guid.Parse("00000000000000000000000000000002"),
        Title = "Tytuł projektu testowego 2",
        Description = "Opis projektu testowego 2",
        Deadline = DateTime.Parse("2024-06-02"),
        Status = "nierozpoczęty",
        CreatedAt = DateTime.Parse("2024-05-02"),
      };
      var input = new PageableInput
      {
        PageNumber = 0,
        PageSize = 10,
        Sorting = new SortPart[]
        {
          new SortPart {Name = "createdAt", IsAscending = false}
        }
      };
      var expected = new PageableResult<Project>
      {
        Items = new Project[]
        {
          new Project
          {
            Id = Guid.Parse("00000000000000000000000000000001"),
            Title = "Tytuł projektu testowego 1",
            Description = "Opis projektu testowego 1",
            Deadline = DateTime.Parse("2024-06-01"),
            Status = ProjectStatus.NotStarted,
            CreatedAt = DateTime.Parse("2024-05-01"),
            Comments = new List<Comment>() { }
          },
          new Project
          {
            Id = Guid.Parse("00000000000000000000000000000002"),
            Title = "Tytuł projektu testowego 2",
            Description = "Opis projektu testowego 2",
            Deadline = DateTime.Parse("2024-06-02"),
            Status = ProjectStatus.NotStarted,
            CreatedAt = DateTime.Parse("2024-05-02"),
            Comments = new List<Comment>() { }
          },
        },
        Pagination = new Pagination { PageNumber = 0, PageSize = 10, TotalElements = 2 },
      };
      await DatabaseHelper.AddElementToCollection(collection, project1);
      await DatabaseHelper.AddElementToCollection(collection, project2);

      var result = await repository.GetAllAsync(input);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task RemoveAsync_Should_remove_project_from_database_when_project_is_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var project = new ProjectDbo
      {
        Id = projectId,
        Title = "Tytuł projektu testowego",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Parse("2024-06-01"),
        Status = "nierozpoczęty",
        CreatedAt = DateTime.Parse("2024-05-01"),
      };
      await DatabaseHelper.AddElementToCollection(collection, project);

      await repository.RemoveAsync(projectId);

      var retrievedDbo = await DatabaseHelper.GetElementFromCollection(collection, projectId);
      var retrievedProject = dboConverter.Convert<Project>(retrievedDbo);
      retrievedProject.Should().BeNull();
    }

    [Test]
    public async Task RemoveAsync_Should_remove_project_from_database_when_project_does_not_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var project = new ProjectDbo
      {
        Id = projectId,
        Title = "Tytuł projektu testowego",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Parse("2024-06-01"),
        Status = "nierozpoczęty",
        CreatedAt = DateTime.Parse("2024-05-01"),
      };
      await DatabaseHelper.AddElementToCollection(collection, project);

      await repository.RemoveAsync(Guid.Parse("00000000000000000000000000000002"));

      var retrievedDbo = await DatabaseHelper.GetElementFromCollection(collection, projectId);
      var retrievedProject = dboConverter.Convert<Project>(retrievedDbo);
      retrievedProject.Should().NotBeNull();
    }

    [Test]
    public async Task ChangeOneAsync_Should_return_changed_project_when_project_is_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var project = new ProjectDbo
      {
        Id = projectId,
        Title = "Tytuł projektu testowego 1",
        Description = "Opis projektu testowego 1",
        Deadline = DateTime.Parse("2024-06-01"),
        Status = "nierozpoczęty",
        CreatedAt = DateTime.Parse("2024-05-01"),
      };
      var changes = new Change<Project>
      {
        Data = new Project
        {
          Title = "Zmieniony tytuł",
          Description = "Zmieniony opis",
          Deadline = DateTime.Parse("2024-07-01")
        },
        Updates = new List<string>
        {
          nameof(Project.Title),
          nameof(Project.Description),
          nameof(Project.Deadline),
        }
      };
      var expected = new Project
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Title = "Zmieniony tytuł",
        Description = "Zmieniony opis",
        Deadline = DateTime.Parse("2024-07-01"),
        Status = ProjectStatus.NotStarted,
        CreatedAt = DateTime.Parse("2024-05-01"),
        Comments = new List<Comment>() { }

      };
      await DatabaseHelper.AddElementToCollection(collection, project);

      var result = await repository.ChangeOneAsync(projectId, changes);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task ChangeOneAsync_Should_return_null_when_project_does_not_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var changes = new Change<Project>
      {
        Data = new Project
        {
          Title = "Zmieniony tytuł",
          Description = "Zmieniony opis",
          Deadline = DateTime.Parse("2024-07-01")
        },
        Updates = new List<string>
        {
          nameof(Project.Title),
          nameof(Project.Description),
          nameof(Project.Deadline),
        }
      };

      var result = await repository.ChangeOneAsync(projectId, changes);

      result.Should().BeNull();
    }
  }
}
