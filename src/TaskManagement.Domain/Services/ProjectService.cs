using DataLayer;
using Domain.Providers;
using Models;
using Models.Statueses;
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

    public async Task<Project> AddAsync(Project project)
    {
      await ValidateStoreProject(project);

      project.Id = GuidProvider.GenetareGuid();
      project.Status = ProjectStatus.NotStarted;
      project.CreatedAt = TimeProvider.GetTime();

      return await repository.StoreAsync(project);
    }

    public async System.Threading.Tasks.Task DeleteAsync(Guid id)
      => await repository.RemoveAsync(id);

    public async Task<Project> EndProjectAsync(Guid projectId)
    {
      var project = await repository.GetByIdAsync(projectId);

      if (project == null)
      {
        return null;
      }

      project.Status = ProjectStatus.Ended;

      var changes = new Change<Project>
      {
        Data = project,
        Updates = new List<string> { nameof(project.Status) }
      };

      return await Patch(projectId, changes);
    }

    public async Task<Project> AddCommentAsync(Guid projectId, Comment comment)
    {
      var project = await repository.GetByIdAsync(projectId);

      if (project == null)
      {
        return null;
      }

      comment.Id = GuidProvider.GenetareGuid();
      comment.CreatedAt = TimeProvider.GetTime();

      project.Comments = project.Comments ?? new List<Comment>();
      project.Comments.Add(comment);

      var changes = new Change<Project>
      {
        Data = project,
        Updates = new List<string> { nameof(project.Comments) }
      };

      return await Patch(projectId, changes);
    }

    public async Task<Project> DeleteCommentAsync(Guid projectId, Guid commentId)
    {
      var project = await repository.GetByIdAsync(projectId);

      if (project == null)
      {
        return null;
      }

      project.Comments = (project.Comments ?? new List<Comment>())
        .Where(s => s.Id != commentId)
        .OrderBy(s => s)
        .ToList();

      var changes = new Change<Project>
      {
        Data = project,
        Updates = new List<string> { nameof(project.Comments) }
      };

      return await Patch(projectId, changes);
    }

    public async Task<Project> DeleteTaskAsync(Guid projectId, Guid taskId)
    {
      var project = await repository.GetByIdAsync(projectId);

      if (project == null)
      {
        return null;
      }

      project.TaskIds = (project.TaskIds ?? new List<Guid>())
        .Where(s => s != taskId)
        .OrderBy(s => s)
        .ToList();

      var changes = new Change<Project>
      {
        Data = project,
        Updates = new List<string> { nameof(project.TaskIds) }
      };

      return await Patch(projectId, changes);
    }

    public async Task<Project> Patch(Guid id, Change<Project> project)
    {
      await ValidateChangeProject(project);

      return await repository.ChangeOneAsync(id, project);
    }


    private async System.Threading.Tasks.Task ValidateStoreProject(Project project)
    {
      if (project == null)
      {
        throw new InvalidDataException("Nie podano żadnych danych !");
      }
      if (string.IsNullOrWhiteSpace(project.Title))
      {
        throw new InvalidDataException("Pozycja 'Tytuł' jest wymagana !");
      }

      if (string.IsNullOrWhiteSpace(project?.Description))
      {
        throw new InvalidDataException("Pozycja 'Opis' jest wymagana !");
      }

      if (project.Priority > 5 || project.Priority < 0)
      {
        throw new InvalidDataException("Priorytet musi mieścić się w zakresie od 0 do 5 !");
      }

      if (project.Deadline < DateTime.Now)
      {
        throw new InvalidDataException("Termin wykonania projektu już minął !");
      }

      var input = new PageableInput() { PageNumber = 0, PageSize = int.MaxValue };
      var allProjects = await repository.GetAllAsync(input);

      if (allProjects.Items.Any(s => s.Title == project.Title))
      {
        throw new InvalidDataException($"Istnieje już projekt o nazwie {project.Title} !");
      }
    }

    private async System.Threading.Tasks.Task ValidateChangeProject(Change<Project> project)
    {
      if (project.Updates.Contains(nameof(Project.Title)))
      {
        var input = new PageableInput() { PageNumber = 0, PageSize = int.MaxValue };
        var allProjects = await repository.GetAllAsync(input);

        if (allProjects.Items.Any(s => s.Title == project.Data.Title))
        {
          throw new InvalidDataException($"Istnieje już projekt o nazwie {project.Data.Title} !");
        }
      }

      if (project.Updates.Contains(nameof(Project.Priority)))
      {
        if (project.Data.Priority > 5 || project.Data.Priority < 1)
        {
          throw new InvalidDataException("Priorytet musi mieścić się w zakresie od 1 do 5 !");
        }
      }

      if (project.Updates.Contains(nameof(Project.Deadline)))
      {
        if (project.Data.Deadline < DateTime.Now)
        {
          throw new InvalidDataException("Termin wykonania projektu już minął !");
        }
      }
    }
  }
}
