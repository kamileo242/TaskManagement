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
    private readonly ITaskPriorityService taskPriorityService;

    public TaskService(
      ITaskRepository repository,
      IProjectService projectService,
      IUserService userService,
      ITaskPriorityService taskPriorityService)
    {
      this.repository = repository;
      this.projectService = projectService;
      this.userService = userService;
      this.taskPriorityService = taskPriorityService;
    }

    public async Task<Models.Task> GetByIdAsync(Guid id)
    => await repository.GetByIdAsync(id);

    public Models.Task GetById(Guid id)
    => repository.GetByIdAsync(id).GetAwaiter().GetResult();

    public async Task<PageableResult<Models.Task>> GetAllAsync(PageableInput input)
      => await repository.GetAllAsync(input);

    public async Task<Models.Task> AddAsync(IHistoryUpdater updater, Guid projectId, Models.Task task)
    {
      task.Id = GuidProvider.GenetareGuid();
      if (string.IsNullOrEmpty(task.Priority))
      {
        task.Priority = taskPriorityService.DefaultPriority;
      }

      await ValidateStoreTask(projectId, task);
      ChangeProjectStatusAndAddTaskId(updater, projectId, task.Id);

      var result = await repository.StoreAsync(task);

      updater.SetObjectId<Models.Task>(result.Id);
      updater.SetChangeDetails(null, result);

      return result;
    }

    public async System.Threading.Tasks.Task DeleteAsync(IHistoryUpdater updater, Guid id)
    {
      updater.SetObjectId<Models.Task>(id);

      await repository.RemoveAsync(id);
    }

    public async Task<Models.Task> RegisterTimeAsync(IHistoryUpdater updater, Guid taskId, int timeInMinutes)
    {
      var task = await GetByIdAsync(taskId);

      if (task == null)
      {
        return null;
      }

      var originalTask = new Models.Task
      {
        Id = task.Id,
        Title = task.Title,
        Deadline = task.Deadline,
        Description = task.Description,
        AssignedPersonId = task.AssignedPersonId,
        Comments = task.Comments,
        CreatedAt = task.CreatedAt,
        Priority = task.Priority,
        SpentTime = task.SpentTime,
        Status = task.Status,
      };

      task.Status = TaskStatus.Started;
      task.SpentTime = task.SpentTime + timeInMinutes;

      var changes = new Change<Models.Task>
      {
        Data = task,
        Updates = new List<string> { nameof(task.SpentTime), nameof(task.Status) }
      };

      var result = await Patch(taskId, changes);

      updater.SetObjectId<Models.Task>(result.Id);
      updater.SetChangeDetails(originalTask, result);

      return result;
    }

    public async Task<Models.Task> AssignPersonToTaskAsync(IHistoryUpdater updater, Guid taskId, Guid userId)
    {
      var task = await GetByIdAsync(taskId);

      if (task == null)
      {
        return null;
      }

      var originalTask = new Models.Task
      {
        Id = task.Id,
        Title = task.Title,
        Deadline = task.Deadline,
        Description = task.Description,
        AssignedPersonId = task.AssignedPersonId,
        Comments = task.Comments,
        CreatedAt = task.CreatedAt,
        Priority = task.Priority,
        SpentTime = task.SpentTime,
        Status = task.Status,
      };

      var exisitng = await userService.GetByIdAsync(userId);

      if (exisitng == null)
      {
        throw new InvalidDataException($"Nie znaleziono użytkownika o Id: {userId}.");
      }

      task.AssignedPersonId = userId;

      var changes = new Change<Models.Task>
      {
        Data = task,
        Updates = new List<string> { nameof(Models.Task.AssignedPersonId) }
      };

      var result = await Patch(taskId, changes);

      updater.SetObjectId<Models.Task>(result.Id);
      updater.SetChangeDetails(originalTask, result);

      return result;
    }

    public async Task<Models.Task> EndTaskStatusAsync(IHistoryUpdater updater, Guid taskId)
    {
      var task = await GetByIdAsync(taskId);

      if (task == null)
      {
        return null;
      }

      var oldStatus = task.Status.Value;
      task.Status = TaskStatus.Ended;

      var changes = new Change<Models.Task>
      {
        Data = task,
        Updates = new List<string> { nameof(Models.Task.Status) }
      };

      var result = await Patch(taskId, changes);

      updater.SetObjectId<Models.Task>(result.Id);
      updater.SetChangeDetailsStatus(oldStatus, result.Status.Value);

      return result;
    }

    public async Task<Models.Task> AddCommentAsync(IHistoryUpdater updater, Guid taskId, Comment comment)
    {
      var task = await repository.GetByIdAsync(taskId);

      if (task == null)
      {
        return null;
      }

      var originalTask = new Models.Task
      {
        Id = task.Id,
        Title = task.Title,
        Deadline = task.Deadline,
        Description = task.Description,
        AssignedPersonId = task.AssignedPersonId,
        Comments = task.Comments,
        CreatedAt = task.CreatedAt,
        Priority = task.Priority,
        SpentTime = task.SpentTime,
        Status = task.Status,
      };

      comment.Id = GuidProvider.GenetareGuid();
      comment.CreatedAt = TimeProvider.GetTime();

      task.Comments = task.Comments ?? new List<Comment>();
      task.Comments.Add(comment);

      var changes = new Change<Models.Task>
      {
        Data = task,
        Updates = new List<string> { nameof(task.Comments) }
      };

      var result = await Patch(taskId, changes);

      updater.SetObjectId<Models.Task>(result.Id);
      updater.SetChangeDetailsList(originalTask, result);

      return result;
    }

    public async Task<Models.Task> DeleteCommentAsync(IHistoryUpdater updater, Guid taskId, Guid commentId)
    {
      var task = await repository.GetByIdAsync(taskId);

      if (task == null)
      {
        return null;
      }

      var originalTask = new Models.Task
      {
        Id = task.Id,
        Title = task.Title,
        Deadline = task.Deadline,
        Description = task.Description,
        AssignedPersonId = task.AssignedPersonId,
        Comments = task.Comments,
        CreatedAt = task.CreatedAt,
        Priority = task.Priority,
        SpentTime = task.SpentTime,
        Status = task.Status,
      };

      task.Comments = (task.Comments ?? new List<Comment>())
        .Where(s => s.Id != commentId)
        .OrderBy(s => s)
        .ToList();

      var changes = new Change<Models.Task>
      {
        Data = task,
        Updates = new List<string> { nameof(task.Comments) }
      };

      var result = await Patch(taskId, changes);

      updater.SetObjectId<Models.Task>(result.Id);
      updater.SetChangeDetailsList(originalTask, result);

      return result;
    }

    public async Task<Models.Task> PatchAsync(IHistoryUpdater updater, Guid id, Change<Models.Task> task)
    {
      var existingTask = await repository.GetByIdAsync(id);

      if (existingTask == null)
      {
        return null;
      }

      var result = await Patch(id, task);

      updater.SetObjectId<Models.Task>(result.Id);
      updater.SetChangeDetails(existingTask, result);

      return result;
    }


    private async Task<Models.Task> Patch(Guid id, Change<Models.Task> task)
    {
      await ValidateChangeTask(task);

      return await repository.ChangeOneAsync(id, task);
    }

    private async System.Threading.Tasks.Task ValidateStoreTask(Guid projectId, Models.Task task)
    {
      if (task == null)
      {
        throw new InvalidDataException("Nie podano żadnych danych.");
      }

      if (string.IsNullOrWhiteSpace(task.Title))
      {
        throw new InvalidDataException("Pozycja 'Tytuł' jest wymagana.");
      }

      var input = new PageableInput() { PageNumber = 0, PageSize = int.MaxValue };
      var allTasks = await repository.GetAllAsync(input);

      if (allTasks.Items.Any(s => s.Title == task.Title))
      {
        throw new InvalidDataException($"Istnieje już zadanie o nazwie {task.Title}.");
      }

      if (task.Deadline < DateTime.Now)
      {
        throw new InvalidDataException("Termin wykonania zadania już minął.");
      }

      taskPriorityService.ValidatePriorityId(task.Priority);

      var project = await projectService.GetByIdAsync(projectId);

      if (project == null)
      {
        throw new InvalidDataException($"Nie znlaziono projektu o identyfikatorze {projectId}.");
      }

      if (project.Status == ProjectStatus.Ended)
      {
        throw new InvalidDataException("Nie można dodać zadania do zakończonego projektu.");
      }
    }

    private async System.Threading.Tasks.Task ValidateChangeTask(Change<Models.Task> task)
    {
      if (task.Updates.Contains(nameof(Models.Task.Priority), StringComparer.InvariantCultureIgnoreCase))
      {
        taskPriorityService.ValidatePriorityId(task.Data.Priority);
      }

      if (task.Updates.Contains(nameof(Models.Task.Deadline), StringComparer.InvariantCultureIgnoreCase))
      {
        if (task.Data.Deadline < DateTime.Now)
        {
          throw new InvalidDataException("Termin wykonania projektu już minął.");
        }
      }

      if (task.Updates.Contains(nameof(Models.Task.Title), StringComparer.InvariantCultureIgnoreCase))
      {
        var input = new PageableInput() { PageNumber = 0, PageSize = int.MaxValue };
        var allTasks = await repository.GetAllAsync(input);

        if (allTasks.Items.Any(s => s.Title == task.Data.Title))
        {
          throw new InvalidDataException($"Istnieje już zadanie o nazwie {task.Data.Title}.");
        }
      }
    }

    private async void ChangeProjectStatusAndAddTaskId(IHistoryUpdater updater, Guid projectId, Guid taskId)
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

      await projectService.PatchAsync(updater, projectId, changes);
    }
  }
}
