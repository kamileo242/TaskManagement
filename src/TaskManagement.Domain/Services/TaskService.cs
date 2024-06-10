using DataLayer;
using Domain.Providers;
using Models;
using Models.Statueses;
using TaskStatus = Models.Statueses.TaskStatus;
using TimeProvider = Domain.Providers.TimeProvider;

namespace Domain.Services
{
  public class TaskService : ITaskService
  {
    private readonly ITaskRepository repository;
    private readonly IProjectService projectService;
    private readonly IUserService userService;

    public TaskService(ITaskRepository repository, IProjectService projectService, IUserService userService)
    {
      this.repository = repository;
      this.projectService = projectService;
      this.userService = userService;
    }

    public async Task<Models.Task> GetByIdAsync(Guid id)
    => await repository.GetByIdAsync(id);

    public Models.Task GetById(Guid id)
    => repository.GetByIdAsync(id).GetAwaiter().GetResult();

    public async Task<PageableResult<Models.Task>> GetAllAsync(PageableInput input)
      => await repository.GetAllAsync(input);

    public async Task<Models.Task> AddAsync(Guid projectId, Models.Task task)
    {
      await ValidateStoreTask(projectId, task);

      task.Id = GuidProvider.GenetareGuid();
      task.Status = Models.Statueses.TaskStatus.NotStarted;
      task.CreatedAt = TimeProvider.GetTime();

      ChangeProjectStatusAndAddTaskId(projectId, task.Id);

      return await repository.StoreAsync(task);
    }

    public async System.Threading.Tasks.Task DeleteAsync(Guid id)
      => await repository.RemoveAsync(id);

    public async Task<Models.Task> RegisterTimeAsync(Guid taskId, int timeInMinutes)
    {
      var task = await GetByIdAsync(taskId);

      if (task == null)
      {
        return null;
      }

      task.Status = TaskStatus.Started;
      task.SpentTime = task.SpentTime + timeInMinutes;

      var changes = new Change<Models.Task>
      {
        Data = task,
        Updates = new List<string> { nameof(task.SpentTime), nameof(task.Status) }
      };

      return await Patch(taskId, changes);
    }

    public async Task<Models.Task> AssignPersonToTaskAsync(Guid taskId, Guid userId)
    {
      var task = await GetByIdAsync(taskId);

      if (task == null)
      {
        return null;
      }

      var exisitng = await userService.GetByIdAsync(userId);

      if (exisitng == null)
      {
        throw new InvalidDataException($"Nie znaleziono użytkownika o Id: {userId}");
      }

      task.AssignedPersonId = userId;

      var changes = new Change<Models.Task>
      {
        Data = task,
        Updates = new List<string> { nameof(Models.Task.AssignedPersonId) }
      };

      return await Patch(taskId, changes);
    }

    public async Task<Models.Task> EndTaskStatusAsync(Guid taskId)
    {
      var task = await GetByIdAsync(taskId);

      if (task == null)
      {
        return null;
      }

      task.Status = TaskStatus.Ended;

      var changes = new Change<Models.Task>
      {
        Data = task,
        Updates = new List<string> { nameof(Models.Task.Status) }
      };

      return await Patch(taskId, changes);
    }

    public async Task<Models.Task> AddCommentAsync(Guid taskId, Comment comment)
    {
      var task = await repository.GetByIdAsync(taskId);

      if (task == null)
      {
        return null;
      }

      comment.Id = GuidProvider.GenetareGuid();
      comment.CreatedAt = TimeProvider.GetTime();

      task.Comments = task.Comments ?? new List<Comment>();
      task.Comments.Add(comment);

      var changes = new Change<Models.Task>
      {
        Data = task,
        Updates = new List<string> { nameof(task.Comments) }
      };

      return await Patch(taskId, changes);
    }

    public async Task<Models.Task> DeleteCommentAsync(Guid taskId, Guid commentId)
    {
      var task = await repository.GetByIdAsync(taskId);

      if (task == null)
      {
        return null;
      }

      task.Comments = (task.Comments ?? new List<Comment>())
        .Where(s => s.Id != commentId)
        .OrderBy(s => s)
        .ToList();

      var changes = new Change<Models.Task>
      {
        Data = task,
        Updates = new List<string> { nameof(task.Comments) }
      };

      return await Patch(taskId, changes);
    }

    public async Task<Models.Task> Patch(Guid id, Change<Models.Task> task)
    {
      await ValidateChangeTask(task);

      return await repository.ChangeOneAsync(id, task);
    }

    private async System.Threading.Tasks.Task ValidateStoreTask(Guid projectId, Models.Task task)
    {
      if (task == null)
      {
        throw new InvalidDataException("Nie podano żadnych danych!");
      }

      if (string.IsNullOrWhiteSpace(task.Title))
      {
        throw new InvalidDataException("Pozycja 'Tytuł' jest wymagana!");
      }

      var input = new PageableInput() { PageNumber = 0, PageSize = int.MaxValue };
      var allTasks = await repository.GetAllAsync(input);

      if (allTasks.Items.Any(s => s.Title == task.Title))
      {
        throw new InvalidDataException($"Istnieje już zadanie o nazwie {task.Title} !");
      }

      if (task.Priority > 5 || task.Priority < 1)
      {
        throw new InvalidDataException("Priorytet musi mieścić się w zakresie od 1 do 5 !");
      }

      if (task.Deadline < DateTime.Now)
      {
        throw new InvalidDataException("Termin wykonania projektu już minął !");
      }

      var project = await projectService.GetByIdAsync(projectId);

      if (project == null)
      {
        throw new InvalidDataException($"Nie znlaziono projektu o identyfikatorze {projectId}");
      }

      if (project.Status == ProjectStatus.Ended)
      {
        throw new InvalidDataException($"Nie można dodać zadania do zakończonego projektu");
      }
    }

    private async System.Threading.Tasks.Task ValidateChangeTask(Change<Models.Task> task)
    {
      if (task.Updates.Contains(nameof(Models.Task.Priority)))
      {
        if (task.Data.Priority > 5 || task.Data.Priority < 1)
        {
          throw new InvalidDataException("Priorytet musi mieścić się w zakresie od 1 do 5 !");
        }
      }

      if (task.Updates.Contains(nameof(Models.Task.Deadline)))
      {
        if (task.Data.Deadline < DateTime.Now)
        {
          throw new InvalidDataException("Termin wykonania projektu już minął !");
        }
      }

      if (task.Updates.Contains(nameof(Models.Task.Title)))
      {
        var input = new PageableInput() { PageNumber = 0, PageSize = int.MaxValue };
        var allTasks = await repository.GetAllAsync(input);

        if (allTasks.Items.Any(s => s.Title == task.Data.Title))
        {
          throw new InvalidDataException($"Istnieje już zadanie o nazwie {task.Data.Title}!");
        }
      }
    }

    private async void ChangeProjectStatusAndAddTaskId(Guid projectId, Guid taskId)
    {
      var project = await projectService.GetByIdAsync(projectId);

      project.TaskIds = (project.TaskIds ?? new List<Guid>())
          .Append(taskId)
          .Distinct()
          .OrderBy(s => s)
          .ToList();

      project.Status = ProjectStatus.Started;

      var changes = new Change<Project>
      {
        Data = project,
        Updates = new List<string> { nameof(project.TaskIds), nameof(project.Status) }
      };

      await projectService.Patch(projectId, changes);
    }
  }
}
