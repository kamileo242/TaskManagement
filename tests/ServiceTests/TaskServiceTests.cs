using DataLayer;
using Domain;
using Domain.Services;
using FluentAssertions;
using Models;
using Models.Statueses;
using Moq;
using Task = System.Threading.Tasks.Task;
using TaskStatus = Models.Statueses.TaskStatus;

namespace ServiceTests
{
  [TestFixture]
  public class TaskServiceTests
  {
    private Mock<ITaskRepository> mockRepository;
    private Mock<IProjectService> mockProjectService;
    private Mock<IUserService> mockUserService;
    private Mock<IHistoryUpdater> mockUpdater;
    private ITaskService taskService;

    [SetUp]
    public void Setup()
    {
      mockRepository = new Mock<ITaskRepository>();
      mockProjectService = new Mock<IProjectService>();
      mockUserService = new Mock<IUserService>();
      mockUpdater = new Mock<IHistoryUpdater>();
      taskService = new TaskService(mockRepository.Object, mockProjectService.Object, mockUserService.Object);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_task_when_valid_id()
    {
      var expected = new Models.Task
      {
        Id = Guid.Parse("00000000000000000000000000000001"),
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      mockRepository.Setup(s => s.GetByIdAsync(expected.Id)).ReturnsAsync(expected);

      var result = await taskService.GetByIdAsync(expected.Id);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetByIdAsync_Should_return_null_when_task_not_found()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync((Models.Task) null);

      var result = await taskService.GetByIdAsync(taskId);

      result.Should().BeNull();
    }

    [Test]
    public async Task GetAllAsync_Should_return_tasks_with_pagination()
    {
      var input = new PageableInput
      {
        PageNumber = 0,
        PageSize = 50,
      };
      var expected = new PageableResult<Models.Task>
      {
        Items = new[]
        {
          new Models.Task
          {
            Id = Guid.Parse("00000000000000000000000000000001"),
            Title = "Zadanie testowe",
            Description = "Opis zadania testowego",
            Priority = 1,
            Deadline = DateTime.Parse("2024-05-05"),
            Status = TaskStatus.NotStarted,
            CreatedAt = DateTime.Parse("2024-04-01"),
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

      var result = await taskService.GetAllAsync(input);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddAsync_Should_return_stored_task_when_valid_data()
    {
      var createdAt = DateTime.Parse("2024-05-05");
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var projectId = Guid.Parse("00000000000000000000000000000002");
      var tasks = new PageableResult<Models.Task>
      {
        Items = Array.Empty<Models.Task>(),
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 0
        }
      };
      var taskToAdd = new Models.Task
      {
        Title = "Zadanie testowy",
        Description = "Opis zadania testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
      };
      var project = new Project
      {
        Id = Guid.Parse("00000000000000000000000000000002"),
        Title = "Zadanie testowy",
        Description = "Opis zadania testowego",
        Deadline = DateTime.Parse("2024-05-05"),
        Status = ProjectStatus.NotStarted
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(tasks);
      mockRepository.Setup(s => s.StoreAsync(It.IsAny<Models.Task>()))
              .ReturnsAsync((Models.Task task) =>
              {
                task.Id = taskId;
                task.CreatedAt = createdAt;
                task.Status = TaskStatus.NotStarted;
                return task;
              });
      mockProjectService.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync(project);
      mockProjectService.Setup(s => s.PatchAsync(mockUpdater.Object, projectId, It.IsAny<Change<Project>>())).ReturnsAsync(It.IsAny<Project>());

      var result = await taskService.AddAsync(mockUpdater.Object, projectId, taskToAdd);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(taskToAdd, o => o
      .Excluding(s => s.Id)
      .Excluding(s => s.CreatedAt)
      .Excluding(s => s.Status));
      result.Id.Should().Be(taskId);
      result.CreatedAt.Should().Be(createdAt);
      result.Status.Should().Be(TaskStatus.NotStarted);
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_missing_title()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var projectId = Guid.Parse("00000000000000000000000000000002");
      var taskToAdd = new Models.Task
      {
        Description = "Opis zadania testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
      };

      var action = async () => await taskService.AddAsync(mockUpdater.Object, projectId, taskToAdd);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Pozycja 'Tytuł' jest wymagana.");
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_task_with_the_same_title_is_already_exist()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var projectId = Guid.Parse("00000000000000000000000000000002");
      var tasks = new PageableResult<Models.Task>
      {
        Items = new[]
        {
          new Models.Task
          {
            Id = taskId,
            Title = "Zadanie testowe",
            Description = "Opis zadania testowego",
            Priority = 1,
            Deadline = DateTime.Parse("2024-05-05"),
            Status = TaskStatus.NotStarted,
            CreatedAt = DateTime.Parse("2024-04-01"),
          }
        },
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 0
        }
      };
      var taskToAdd = new Models.Task
      {
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(tasks);

      var action = async () => await taskService.AddAsync(mockUpdater.Object, projectId, taskToAdd);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage($"Istnieje już zadanie o nazwie {taskToAdd.Title}.");
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_invalid_priority()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var projectId = Guid.Parse("00000000000000000000000000000002");
      var tasks = new PageableResult<Models.Task>
      {
        Items = Array.Empty<Models.Task>(),
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 0
        }
      };
      var taskToAdd = new Models.Task
      {
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 8,
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(tasks);

      var action = async () => await taskService.AddAsync(mockUpdater.Object, projectId, taskToAdd);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Priorytet musi mieścić się w zakresie od 1 do 5.");
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_invalid_deadline()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var projectId = Guid.Parse("00000000000000000000000000000002");
      var tasks = new PageableResult<Models.Task>
      {
        Items = Array.Empty<Models.Task>(),
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 0
        }
      };
      var taskToAdd = new Models.Task
      {
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Deadline = DateTime.Now.AddDays(-3),
        Priority = 1,
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(tasks);

      var action = async () => await taskService.AddAsync(mockUpdater.Object, projectId, taskToAdd);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Termin wykonania zadania już minął.");
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_project_is_not_exist()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var projectId = Guid.Parse("00000000000000000000000000000002");
      var tasks = new PageableResult<Models.Task>
      {
        Items = Array.Empty<Models.Task>(),
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 0
        }
      };
      var taskToAdd = new Models.Task
      {
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(tasks);
      mockProjectService.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync((Project) null);

      var action = async () => await taskService.AddAsync(mockUpdater.Object, projectId, taskToAdd);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage($"Nie znlaziono projektu o identyfikatorze {projectId}.");
    }

    [Test]
    public async Task AddAsync_Should_throw_exception_when_project_is_ended()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var projectId = Guid.Parse("00000000000000000000000000000002");
      var tasks = new PageableResult<Models.Task>
      {
        Items = Array.Empty<Models.Task>(),
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 0
        }
      };
      var taskToAdd = new Models.Task
      {
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Deadline = DateTime.Now.AddDays(3),
        Priority = 1,
      };
      var project = new Project
      {
        Id = Guid.Parse("00000000000000000000000000000002"),
        Title = "Zadanie testowy",
        Description = "Opis zadania testowego",
        Deadline = DateTime.Parse("2024-05-05"),
        Status = ProjectStatus.Ended
      };
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(tasks);
      mockProjectService.Setup(s => s.GetByIdAsync(projectId)).ReturnsAsync(project);

      var action = async () => await taskService.AddAsync(mockUpdater.Object, projectId, taskToAdd);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Nie można dodać zadania do zakończonego projektu.");
    }

    [Test]
    public async Task DeleteAsync_Should_remove_task()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");

      await taskService.DeleteAsync(mockUpdater.Object, taskId);

      mockRepository.Verify(s => s.RemoveAsync(taskId), Times.Once);
    }

    [Test]
    public async Task RegisterTimeAsync_Should_return_updated_task_when_valid_data()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var task = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      var expected = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.Started,
        SpentTime = 320,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync(task);
      mockRepository.Setup(s => s.ChangeOneAsync(taskId, It.IsAny<Change<Models.Task>>())).ReturnsAsync(expected);

      var result = await taskService.RegisterTimeAsync(mockUpdater.Object, taskId, 120);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task RegisterTimeAsync_Should_return_null_when_task_is_not_exist()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync((Models.Task) null);

      var result = await taskService.RegisterTimeAsync(mockUpdater.Object, taskId, 120);

      result.Should().BeNull();
    }

    [Test]
    public async Task AssignPersonToTaskAsync_Should_return_updated_task_when_valid_data()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      var task = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      var user = new User
      {
        Id = userId,
        Name = "Johnny",
        Surname = "Deep",
        Position = "Actor",
        Email = "johnny.deep@example.com",
        PhoneNumber = "123456789"
      };
      var expected = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
        AssignedPersonId = userId,
      };
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync(task);
      mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
      mockRepository.Setup(s => s.ChangeOneAsync(taskId, It.IsAny<Change<Models.Task>>())).ReturnsAsync(expected);

      var result = await taskService.AssignPersonToTaskAsync(mockUpdater.Object, taskId, userId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AssignPersonToTaskAsync_Should_return_null_when_task_is_not_exist()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync((Models.Task) null);

      var result = await taskService.AssignPersonToTaskAsync(mockUpdater.Object, taskId, userId);

      result.Should().BeNull();
    }

    [Test]
    public async Task AssignPersonToTaskAsync_Should_throw_exception_when_user_is_not_exist()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var userId = Guid.Parse("00000000000000000000000000000002");
      var task = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync(task);
      mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((User) null);

      var action = async () => await taskService.AssignPersonToTaskAsync(mockUpdater.Object, taskId, userId);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage($"Nie znaleziono użytkownika o Id: {userId}.");
    }

    [Test]
    public async Task EndTaskStatusAsync_Should_return_updated_task_when_valid_data()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var task = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      var expected = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.Ended,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync(task);
      mockRepository.Setup(s => s.ChangeOneAsync(taskId, It.IsAny<Change<Models.Task>>())).ReturnsAsync(expected);

      var result = await taskService.EndTaskStatusAsync(mockUpdater.Object, taskId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task EndTaskStatusAsync_Should_return_null_when_task_is_not_exist()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync((Models.Task) null);

      var result = await taskService.EndTaskStatusAsync(mockUpdater.Object, taskId);

      result.Should().BeNull();
    }

    [Test]
    public async Task AddCommentAsync_Should_return_updated_task_when_valid_data()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var task = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      var expected = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.Ended,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
        Comments = new List<Comment>
        {
          new Comment
          {
            Author = "Kamil",
            Content = "Treść",
          }
        }
      };
      var newComment = new Comment
      {
        Author = "Kamil",
        Content = "Treść"
      };
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync(task);
      mockRepository.Setup(s => s.ChangeOneAsync(taskId, It.IsAny<Change<Models.Task>>())).ReturnsAsync(expected);

      var result = await taskService.AddCommentAsync(mockUpdater.Object, taskId, newComment);

      var endTestTime = DateTime.Now;
      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected, o => o.
      Excluding(s => s.Comments));
      result.Comments.Should().HaveCount(1);
      result.Comments[0].Author.Should().Be(newComment.Author);
      result.Comments[0].Content.Should().Be(newComment.Content); ;
    }

    [Test]
    public async Task AddCommentAsync_Should_return_null_when_task_is_not_exist()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var newComment = new Comment
      {
        Author = "Kamil",
        Content = "Treść"
      };
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync((Models.Task) null);

      var result = await taskService.AddCommentAsync(mockUpdater.Object, taskId, newComment);

      result.Should().BeNull();
    }

    [Test]
    public async Task DeleteCommentAsync_Should_return_null_when_task_is_not_exist()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var commentId = Guid.Parse("00000000000000000000000000000002");
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync((Models.Task) null);

      var result = await taskService.DeleteCommentAsync(mockUpdater.Object, taskId, commentId);

      result.Should().BeNull();
    }

    [Test]
    public async Task DeleteCommentAsync_Should_return_task_when_comment_is_not_exist()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var commentId = Guid.Parse("00000000000000000000000000000002");
      var task = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync(task);
      mockRepository.Setup(s => s.ChangeOneAsync(taskId, It.IsAny<Change<Models.Task>>())).ReturnsAsync(task);

      var result = await taskService.DeleteCommentAsync(mockUpdater.Object, taskId, commentId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(task);
    }

    [Test]
    public async Task DeleteCommentAsync_Should_return_project_without_commentId_when_comment_is_exist()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var commentId = Guid.Parse("00000000000000000000000000000002");
      var task = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
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
      var expected = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync(task);
      mockRepository.Setup(s => s.ChangeOneAsync(taskId, It.IsAny<Change<Models.Task>>())).ReturnsAsync(expected);

      var result = await taskService.DeleteCommentAsync(mockUpdater.Object, taskId, commentId);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Patch_Should_throw_exception_when_task_with_the_same_title_is_already_exist()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var task = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      var tasks = new PageableResult<Models.Task>
      {
        Items = new Models.Task[] { task },
        Pagination = new Pagination
        {
          PageNumber = 0,
          PageSize = 50,
          TotalElements = 1
        }
      };
      var changes = new Change<Models.Task>
      {
        Data = new Models.Task { Title = "Zadanie testowe", },
        Updates = new List<string> { nameof(Models.Task.Title) }
      };
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync(task);
      mockRepository.Setup(s => s.GetAllAsync(It.IsAny<PageableInput>())).ReturnsAsync(tasks);

      var action = async () => await taskService.PatchAsync(mockUpdater.Object, taskId, changes);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage($"Istnieje już zadanie o nazwie {task.Title}.");
    }

    [Test]
    public async Task Patch_Should_throw_exception_when_invalid_priority()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var task = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      var changes = new Change<Models.Task>
      {
        Data = new Models.Task { Priority = 8, },
        Updates = new List<string> { nameof(Models.Task.Priority) }
      };
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync(task);

      var action = async () => await taskService.PatchAsync(mockUpdater.Object, taskId, changes);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Priorytet musi mieścić się w zakresie od 1 do 5.");
    }

    [Test]
    public async Task Patch_Should_throw_exception_when_invalid_deadline()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var task = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 1,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      var changes = new Change<Models.Task>
      {
        Data = new Models.Task { Deadline = DateTime.Now.AddDays(-3), },
        Updates = new List<string> { nameof(Models.Task.Deadline) }
      };
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync(task);

      var action = async () => await taskService.PatchAsync(mockUpdater.Object, taskId, changes);

      var exception = await action.Should().ThrowAsync<InvalidDataException>();
      exception.WithMessage("Termin wykonania projektu już minął.");
    }

    [Test]
    public async Task Patch_Should_return_null_when_task_is_not_exist()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var changes = new Change<Models.Task>
      {
        Data = new Models.Task { Priority = 2, },
        Updates = new List<string> { nameof(Models.Task.Priority) }
      };

      var result = await taskService.PatchAsync(mockUpdater.Object, taskId, changes);

      result.Should().BeNull();
    }

    [Test]
    public async Task Patch_Should_return_updated_task_when_valid_data()
    {
      var taskId = Guid.Parse("00000000000000000000000000000001");
      var expected = new Models.Task
      {
        Id = taskId,
        Title = "Zadanie testowe",
        Description = "Opis zadania testowego",
        Priority = 2,
        Deadline = DateTime.Parse("2024-05-05"),
        Status = TaskStatus.NotStarted,
        SpentTime = 200,
        CreatedAt = DateTime.Parse("2024-04-01"),
      };
      var changes = new Change<Models.Task>
      {
        Data = new Models.Task { Priority = 2, },
        Updates = new List<string> { nameof(Models.Task.Priority) }
      };
      mockRepository.Setup(s => s.GetByIdAsync(taskId)).ReturnsAsync(expected);
      mockRepository.Setup(s => s.ChangeOneAsync(taskId, It.IsAny<Change<Models.Task>>())).ReturnsAsync(expected);

      var result = await taskService.PatchAsync(mockUpdater.Object, taskId, changes);

      result.Should().NotBeNull();
      result.Should().BeEquivalentTo(expected);
    }
  }
}
