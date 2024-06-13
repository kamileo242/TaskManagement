using DataLayer;
using Domain;
using Domain.Services;
using FluentAssertions;
using Models;
using Models.Statueses;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace ServiceTests
{
  [TestFixture]
  public class ProjectServiceTests
  {
    private Mock<IProjectRepository> mockRepository;
    private IProjectService projectService;

    [SetUp]
    public void Setup()
    {
      mockRepository = new Mock<IProjectRepository>();
      projectService = new ProjectService(mockRepository.Object);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_project_when_valid_id()
    {
      var expected = new Project
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = ProjectStatus.NotStarted
      };
      mockRepository.Setup(s => s.GetByIdAsync(expected.Id)).ReturnsAsync(expected);

      var result = await projectService.GetByIdAsync(expected.Id);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_null_when_project_not_found()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      mockRepository.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync((Project) null);

      var result = await projectService.GetByIdAsync(projectId);

      result.Should().BeNull();
    }

    [Test]
    public async Task GetAllAsync_Should_return_projects_with_pagination()
    {
      var input = new PageableInput
      {
        PageNumber = 0,
        PageSize = 50,
      };
      var expected = new PageableResult<Project>
      {
        Items = new Project[]
        {
          new Project
          {
            Id = Guid.Parse("00000000000000000000000000000001"),
            Title = "Projekt testowy",
            Description = "Opis projektu testowego",
            Priority = 1,
            Deadline = DateTime.Parse("2024-05-05"),
            Status = ProjectStatus.NotStarted
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

      var result = await projectService.GetAllAsync(input);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddAsync_Should_return_stored_project_when_valid_data()
    {
      var createdAt = DateTime.Parse("2024-05-05");
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var projects = new PageableResult<Project>
      {
        Items = Array.Empty<Project>(),
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 0
        }
      };
      var projectToAdd = new Project
      {
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(projects);
      mockRepository.Setup(s => s.StoreAsync(It.IsAny<Project>()))
              .ReturnsAsync((Project project) =>
              {
                project.Id = projectId;
                project.CreatedAt = createdAt;
                project.Status = ProjectStatus.NotStarted;
                return project;
              });

      var result = await projectService.AddAsync(projectToAdd);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(projectToAdd, o => o
      .Excluding(s => s.Id)
      .Excluding(s => s.CreatedAt)
      .Excluding(s => s.Status));
      result.Id.Should().Be(projectId);
      result.CreatedAt.Should().Be(createdAt);
      result.Status.Should().Be(ProjectStatus.NotStarted);
    }

    [Test]
    public async Task AddAsync_Should_trow_exception_when_project_not_contains_title()
    {
      var projectToAdd = new Project
      {
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
      };

      var action = async () => await projectService.AddAsync(projectToAdd);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Pozycja 'Tytuł' jest wymagana.");
    }

    [Test]
    public async Task AddAsync_Should_trow_exception_when_project_not_contains_description()
    {
      var projectToAdd = new Project
      {
        Title = "Projekt testowy",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
      };

      var action = async () => await projectService.AddAsync(projectToAdd);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Pozycja 'Opis' jest wymagana.");
    }

    [Test]
    public async Task AddAsync_Should_trow_exception_when_invalid_priority()
    {
      var projectToAdd = new Project
      {
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 6,
      };

      var action = async () => await projectService.AddAsync(projectToAdd);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Priorytet musi mieścić się w zakresie od 0 do 5.");
    }

    [Test]
    public async Task AddAsync_Should_trow_exception_when_invallid_deadline()
    {
      var projectToAdd = new Project
      {
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(-3),
        Priority = 1,
      };

      var action = async () => await projectService.AddAsync(projectToAdd);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Termin wykonania projektu już minął.");
    }

    [Test]
    public async Task AddAsync_Should_trow_exception_when_project_with_the_same_title_already_exist()
    {
      var createdAt = DateTime.Parse("2024-05-05");
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var projectToAdd = new Project
      {
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
      };
      var projects = new PageableResult<Project>
      {
        Items = new Project[]
        {
          new Project
          {
            Id = projectId,
            Title = "Projekt testowy",
            Description = "Opis projektu testowego",
            Deadline = DateTime.Now.AddDays(3),
            Priority = 1,
            CreatedAt = createdAt,
            Status = ProjectStatus.NotStarted,
          }
        },
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 1
        }
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(projects);

      var action = async () => await projectService.AddAsync(projectToAdd);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage($"Istnieje już projekt o nazwie {projectToAdd.Title}.");
    }

    [Test]
    public async Task DeleteAsync_Should_remove_project()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");

      await projectService.DeleteAsync(projectId);

      mockRepository.Verify(s => s.RemoveAsync(projectId), Times.Once);
    }

    [Test]
    public async Task EndProjectAsync_Should_return_project_with_status_ended()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var expected = new Project
      {
        Id = projectId,
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
        CreatedAt = DateTime.Now,
        Status = ProjectStatus.NotStarted,
      };
      mockRepository.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync(expected);
      mockRepository.Setup(s => s.ChangeOneAsync(projectId, It.IsAny<Change<Project>>())).ReturnsAsync(expected);

      var result = await projectService.EndProjectAsync(projectId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected, o => o.Excluding(s => s.Status));
      result.Status.Should().Be(ProjectStatus.Ended);
    }

    [Test]
    public async Task EndProjectAsync_Should_return_null_when_project_is_not_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");

      mockRepository.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync((Project) null);

      var result = await projectService.EndProjectAsync(projectId);

      result.Should().BeNull();
    }

    [Test]
    public async Task AddCommentAsync_Should_return_null_when_project_is_not_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var newComment = new Comment
      {
        Author = "Kamil",
        Content = "Treść"
      };
      mockRepository.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync((Project) null);

      var result = await projectService.AddCommentAsync(projectId, newComment);

      result.Should().BeNull();
    }

    [Test]
    public async Task AddCommentAsync_Should_return_project_with_comment_when_project_valid_data()
    {
      var startTestTime = DateTime.Now;
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var expected = new Project
      {
        Id = projectId,
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
        CreatedAt = DateTime.Now,
        Status = ProjectStatus.NotStarted,
      };
      var newComment = new Comment
      {
        Author = "Kamil",
        Content = "Treść"
      };
      mockRepository.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync(expected);
      mockRepository.Setup(s => s.ChangeOneAsync(projectId, It.IsAny<Change<Project>>())).ReturnsAsync(expected);

      var result = await projectService.AddCommentAsync(projectId, newComment);

      var endTestTime = DateTime.Now;
      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected, o => o.
      Excluding(s => s.Comments));
      result.Comments.Should().HaveCount(1);
      result.Comments[0].Author.Should().Be(newComment.Author);
      result.Comments[0].Content.Should().Be(newComment.Content);
      result.Comments[0].CreatedAt.Should().BeAfter(startTestTime);
      result.Comments[0].CreatedAt.Should().BeBefore(endTestTime);
    }

    [Test]
    public async Task DeleteCommentAsync_Should_return_null_when_project_is_not_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var commentId = Guid.Parse("00000000000000000000000000000002");
      mockRepository.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync((Project) null);

      var result = await projectService.DeleteCommentAsync(projectId, commentId);

      result.Should().BeNull();
    }

    [Test]
    public async Task DeleteCommentAsync_Should_return_project_when_comment_is_not_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var commentId = Guid.Parse("00000000000000000000000000000002");
      var expected = new Project
      {
        Id = projectId,
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
        CreatedAt = DateTime.Now,
        Status = ProjectStatus.NotStarted,
      };
      mockRepository.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync(expected);
      mockRepository.Setup(s => s.ChangeOneAsync(projectId, It.IsAny<Change<Project>>())).ReturnsAsync(expected);

      var result = await projectService.DeleteCommentAsync(projectId, commentId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task DeleteCommentAsync_Should_return_project_without_commentId_when_comment_is_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var commentId = Guid.Parse("00000000000000000000000000000002");
      var project = new Project
      {
        Id = projectId,
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
        CreatedAt = DateTime.Now,
        Status = ProjectStatus.NotStarted,
        Comments = new List<Comment>
        {
          new Comment
          {
            Id = commentId,
            CreatedAt = DateTime.Parse("2024-05-05"),
            Author = "Kamil",
            Content = "Treść"
          }
        }
      };
      var expected = new Project
      {
        Id = projectId,
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
        CreatedAt = DateTime.Now,
        Status = ProjectStatus.NotStarted,
      };
      mockRepository.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync(expected);
      mockRepository.Setup(s => s.ChangeOneAsync(projectId, It.IsAny<Change<Project>>())).ReturnsAsync(expected);

      var result = await projectService.DeleteCommentAsync(projectId, commentId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task DeleteTaskAsync_Should_return_null_when_project_is_not_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var taskId = Guid.Parse("00000000000000000000000000000002");
      mockRepository.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync((Project) null);

      var result = await projectService.DeleteTaskAsync(projectId, taskId);

      result.Should().BeNull();
    }

    [Test]
    public async Task DeleteTaskAsync_Should_return_project_when_task_is_not_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var taskId = Guid.Parse("00000000000000000000000000000002");
      var expected = new Project
      {
        Id = projectId,
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
        CreatedAt = DateTime.Now,
        Status = ProjectStatus.NotStarted,
      };
      mockRepository.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync(expected);
      mockRepository.Setup(s => s.ChangeOneAsync(projectId, It.IsAny<Change<Project>>())).ReturnsAsync(expected);

      var result = await projectService.DeleteTaskAsync(projectId, taskId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task DeleteTaskAsync_Should_return_project_without_taskId_when_comment_is_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var taskId = Guid.Parse("00000000000000000000000000000002");
      var project = new Project
      {
        Id = projectId,
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
        CreatedAt = DateTime.Now,
        Status = ProjectStatus.NotStarted,
        TaskIds = new List<Guid> { taskId }
      };
      var expected = new Project
      {
        Id = projectId,
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
        CreatedAt = DateTime.Now,
        Status = ProjectStatus.NotStarted,
      };
      mockRepository.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync(expected);
      mockRepository.Setup(s => s.ChangeOneAsync(projectId, It.IsAny<Change<Project>>())).ReturnsAsync(expected);

      var result = await projectService.DeleteTaskAsync(projectId, taskId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Patch_Should_throw_exception_when_project_with_the_same_title_is_already_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var project = new Project
      {
        Id = projectId,
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
        CreatedAt = DateTime.Now,
        Status = ProjectStatus.NotStarted,
      };
      var projects = new PageableResult<Project>
      {
        Items = new Project[] { project },
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 1
        }
      };
      var changes = new Change<Project>
      {
        Data = new Project { Title = "Projekt testowy", },
        Updates = new List<string> { nameof(Project.Title) }
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(projects);

      var action = async () => await projectService.Patch(projectId, changes);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage($"Istnieje już projekt o nazwie {project.Title}.");
    }

    [Test]
    public async Task Patch_Should_throw_exception_when_invalid_priority()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var projects = new PageableResult<Project>
      {
        Items = Array.Empty<Project>(),
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 0
        }
      };
      var changes = new Change<Project>
      {
        Data = new Project { Priority = 7, },
        Updates = new List<string> { nameof(Project.Priority) }
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(projects);

      var action = async () => await projectService.Patch(projectId, changes);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Priorytet musi mieścić się w zakresie od 1 do 5.");
    }

    [Test]
    public async Task Patch_Should_throw_exception_when_invalid_deadline()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var projects = new PageableResult<Project>
      {
        Items = Array.Empty<Project>(),
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 0
        }
      };
      var changes = new Change<Project>
      {
        Data = new Project { Deadline = DateTime.Now.AddDays(-3), },
        Updates = new List<string> { nameof(Project.Deadline) }
      };

      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(projects);

      var action = async () => await projectService.Patch(projectId, changes);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Termin wykonania projektu już minął.");
    }

    [Test]
    public async Task Patch_Should_return_null_when_project_is_not_exist()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var changes = new Change<Project>
      {
        Data = new Project { Description = "Opis", },
        Updates = new List<string> { nameof(Project.Description) }
      };
      mockRepository.Setup(s => s.ChangeOneAsync(projectId, It.IsAny<Change<Project>>())).ReturnsAsync((Project) null);

      var result = await projectService.Patch(projectId, changes);

      result.Should().BeNull();
    }

    [Test]
    public async Task Patch_Should_return_changed_project_when_valid_data()
    {
      var projectId = Guid.Parse("00000000000000000000000000000001");
      var projects = new PageableResult<Project>
      {
        Items = Array.Empty<Project>(),
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 1
        }
      };
      var changes = new Change<Project>
      {
        Data = new Project { Title = "Projekt testowy", },
        Updates = new List<string> { nameof(Project.Title) }
      };
      var expected = new Project
      {
        Id = projectId,
        Title = "Projekt testowy",
        Description = "Opis projektu testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
        CreatedAt = DateTime.Now,
        Status = ProjectStatus.NotStarted,
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(projects);
      mockRepository.Setup(s => s.ChangeOneAsync(projectId, It.IsAny<Change<Project>>())).ReturnsAsync(expected);

      var result = await projectService.Patch(projectId, changes);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }
  }
}
