using Models;
using Task = System.Threading.Tasks.Task;

namespace Domain
{
  /// <summary>
  /// Zbiór operacji zapisujących historię zmian dla operacji wykonywanych na projektach
  /// </summary>
  public interface IProjectServiceWithHistory
  {
    /// <summary>
    /// Pobierz projekt po id
    /// </summary>
    /// <param name="id">Identyfikator projektu</param>
    /// <returns>Projekt</returns>
    Task<Project> GetByIdAsync(Guid id);

    /// <summary>
    /// Pobierz wszystkie projekty
    /// </summary>
    /// <param name="input">Dane podziału na strony</param>
    /// <returns>Lista projektów</returns>
    Task<PageableResult<Project>> GetAllAsync(PageableInput input);

    /// <summary>
    /// Dodaj nowy projekt
    /// </summary>
    /// <param name="project">Projekt</param>
    /// <returns>Nowe projekt</returns>
    Task<Project> AddAsync(OperationContext context, Project project);

    /// <summary>
    /// Usuń projekt
    /// </summary>
    /// <param name="id">Identyfikator projektu</param>
    Task DeleteAsync(OperationContext context, Guid id);

    /// <summary>
    /// Dodaj komentarz do projektu
    /// </summary>
    /// <param name="projectId">Identyfikator projektu</param>
    /// <param name="comment">Treść komentarza</param>
    /// <returns>Projekt z dodanym komentarzem</returns>
    Task<Project> AddCommentAsync(OperationContext context, Guid projectId, Comment comment);

    /// <summary>
    /// Usuń komentarz z projektu
    /// </summary>
    /// <param name="projectId">Identyfikator projektu</param>
    /// <param name="commentId">Identyfikator komentarza</param>
    /// <returns>Projekt z usuniętym komentarzem</returns>
    Task<Project> DeleteCommentAsync(OperationContext context, Guid projectId, Guid commentId);

    /// <summary>
    /// Usuń zadanie z projektu
    /// </summary>
    /// <param name="projectId">Identyfikator projektu</param>
    /// <param name="taskId">Identyfikator zadania</param>
    /// <returns>Projekt z usuniętym zadaniem</returns>
    Task<Project> DeleteTaskAsync(OperationContext context, Guid projectId, Guid taskId);

    /// <summary>
    /// Zmień status projektu
    /// </summary>
    /// <param name="projectId">Identyfikator projektu</param>
    /// <returns>Projekt ze zmienionym statusem</returns>
    Task<Project> EndProjectAsync(OperationContext context, Guid projectId);

    /// <summary>
    /// Zmień wybrane właściwości projektu
    /// </summary>
    /// <param name="project">Dane projektu do modyfikacji</param>
    /// <returns>Zmodyfikowany projekt</returns>
    Task<Project> PatchAsync(OperationContext context, Guid id, Change<Project> project);
  }
}
