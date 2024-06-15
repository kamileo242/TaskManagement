using DataLayer;
using Domain.Providers;
using Models;
using Models.Statueses;
using Task = System.Threading.Tasks.Task;
using TimeProvider = Domain.Providers.TimeProvider;

namespace Domain.Services
{
  public class ProjectService : IProjectService
  {
    private readonly IProjectRepository repository;

    public ProjectService(IProjectRepository repository)
    {
      this.repository = repository;
    }

    public async Task<Project> GetByIdAsync(Guid id)
    => await repository.GetByIdAsync(id);

    public async Task<PageableResult<Project>> GetAllAsync(PageableInput input)
      => await repository.GetAllAsync(input);

    public async Task<Project> AddAsync(IHistoryUpdater updater, Project project)
    {
      await ValidateStoreProject(project);

      project.Id = GuidProvider.GenetareGuid();
      project.Status = ProjectStatus.NotStarted;
      project.CreatedAt = TimeProvider.GetTime();

      var result = await repository.StoreAsync(project);

      updater.SetObjectId<Project>(result.Id);
      updater.SetChangeDetails(null, result);

      return result;
    }

    public async Task DeleteAsync(IHistoryUpdater updater, Guid id)
    {
      updater.SetObjectId<Project>(id);

      await repository.RemoveAsync(id);
    }

    public async Task<Project> EndProjectAsync(IHistoryUpdater updater, Guid projectId)
    {
      var project = await repository.GetByIdAsync(projectId);

      if (project == null)
      {
        return null;
      }
      var oldStatus = project.Status.Value;

      project.Status = ProjectStatus.Ended;

      var changes = new Change<Project>
      {
        Data = project,
        Updates = new List<string> { nameof(project.Status) }
      };

      var result = await Patch(projectId, changes);

      updater.SetObjectId<Project>(result.Id);
      updater.SetChangeDetailsStatus(oldStatus, result.Status.Value);

      return result;
    }

    public async Task<Project> AddCommentAsync(IHistoryUpdater updater, Guid projectId, Comment comment)
    {
      var project = await repository.GetByIdAsync(projectId);

      if (project == null)
      {
        return null;
      }
      var originalProject = new Project
      {
        Id = project.Id,
        Title = project.Title,
        Description = project.Description,
        Deadline = project.Deadline,
        Comments = project.Comments,
        CreatedAt = project.CreatedAt,
        Status = project.Status,
        TaskIds = project.TaskIds,
      };

      comment.Id = GuidProvider.GenetareGuid();
      comment.CreatedAt = TimeProvider.GetTime();

      project.Comments = project.Comments ?? new List<Comment>();
      project.Comments.Add(comment);

      var changes = new Change<Project>
      {
        Data = project,
        Updates = new List<string> { nameof(project.Comments) }
      };

      var result = await Patch(projectId, changes);

      updater.SetObjectId<Project>(result.Id);
      updater.SetChangeDetailsList(originalProject, result);

      return result;
    }

    public async Task<Project> DeleteCommentAsync(IHistoryUpdater updater, Guid projectId, Guid commentId)
    {
      var project = await repository.GetByIdAsync(projectId);

      if (project == null)
      {
        return null;
      }

      var originalProject = new Project
      {
        Id = project.Id,
        Title = project.Title,
        Description = project.Description,
        Deadline = project.Deadline,
        Comments = project.Comments,
        CreatedAt = project.CreatedAt,
        Status = project.Status,
        TaskIds = project.TaskIds,
      };

      project.Comments = (project.Comments ?? new List<Comment>())
        .Where(s => s.Id != commentId)
        .OrderBy(s => s)
        .ToList();

      var changes = new Change<Project>
      {
        Data = project,
        Updates = new List<string> { nameof(project.Comments) }
      };

      var result = await Patch(projectId, changes);

      updater.SetObjectId<Project>(result.Id);
      updater.SetChangeDetailsList(originalProject, result);

      return result;
    }

    public async Task<Project> DeleteTaskAsync(IHistoryUpdater updater, Guid projectId, Guid taskId)
    {
      var project = await repository.GetByIdAsync(projectId);

      if (project == null)
      {
        return null;
      }

      var originalProject = new Project
      {
        Id = project.Id,
        Title = project.Title,
        Description = project.Description,
        Deadline = project.Deadline,
        Comments = project.Comments,
        CreatedAt = project.CreatedAt,
        Status = project.Status,
        TaskIds = project.TaskIds,
      };

      project.TaskIds = (project.TaskIds ?? new List<Guid>())
        .Where(s => s != taskId)
        .OrderBy(s => s)
        .ToList();

      var changes = new Change<Project>
      {
        Data = project,
        Updates = new List<string> { nameof(project.TaskIds) }
      };

      var result = await Patch(projectId, changes);

      updater.SetObjectId<Project>(result.Id);
      updater.SetChangeDetailsList(originalProject, result);

      return result;
    }

    public async Task<Project> PatchAsync(IHistoryUpdater updater, Guid id, Change<Project> project)
    {
      var existingProject = await repository.GetByIdAsync(id);

      if (existingProject == null)
      {
        return null;
      }

      var result = await Patch(id, project);

      updater.SetObjectId<Project>(result.Id);
      updater.SetChangeDetails(existingProject, result);

      return result;
    }


    private async Task<Project> Patch(Guid id, Change<Project> project)
    {
      await ValidateChangeProject(project);

      return await repository.ChangeOneAsync(id, project);
    }

    private async Task ValidateStoreProject(Project project)
    {
      if (project == null)
      {
        throw new InvalidDataException("Nie podano żadnych danych.");
      }
      if (string.IsNullOrWhiteSpace(project.Title))
      {
        throw new InvalidDataException("Pozycja 'Tytuł' jest wymagana.");
      }

      if (string.IsNullOrWhiteSpace(project?.Description))
      {
        throw new InvalidDataException("Pozycja 'Opis' jest wymagana.");
      }

      if (project.Deadline < DateTime.Now)
      {
        throw new InvalidDataException("Termin wykonania projektu już minął.");
      }

      var input = new PageableInput() { PageNumber = 0, PageSize = int.MaxValue };
      var allProjects = await repository.GetAllAsync(input);

      if (allProjects.Items.Any(s => s.Title == project.Title))
      {
        throw new InvalidDataException($"Istnieje już projekt o nazwie {project.Title}.");
      }
    }

    private async Task ValidateChangeProject(Change<Project> project)
    {
      if (project.Updates.Contains(nameof(Project.Title), StringComparer.InvariantCultureIgnoreCase))
      {
        var input = new PageableInput() { PageNumber = 0, PageSize = int.MaxValue };
        var allProjects = await repository.GetAllAsync(input);

        if (allProjects.Items.Any(s => s.Title == project.Data.Title))
        {
          throw new InvalidDataException($"Istnieje już projekt o nazwie {project.Data.Title}.");
        }
      }

      if (project.Updates.Contains(nameof(Project.Deadline), StringComparer.InvariantCultureIgnoreCase))
      {
        if (project.Data.Deadline < DateTime.Now)
        {
          throw new InvalidDataException("Termin wykonania projektu już minął.");
        }
      }
    }
  }
}
