using Models;

namespace Domain
{
  /// <summary>
  /// Zbiór operacji wykonywanych na serwisie zadań
  /// </summary>
  public interface ITaskService
  {
    /// <summary>
    /// Pobierz zadanie po id
    /// </summary>
    /// <param name="id">Identyfikator zadania</param>
    /// <returns>Zadanie</returns>
    Task<Models.Task> GetByIdAsync(Guid id);

    /// <summary>
    /// Pobierz zadanie po id
    /// </summary>
    /// <param name="id">Identyfikator zadania</param>
    /// <returns>Zadanie</returns>
    Models.Task GetById(Guid id);

    /// <summary>
    /// Pobierz wszystkie zadania
    /// </summary>
    /// <returns>Lista zadań</returns>
    Task<PageableResult<Models.Task>> GetAllAsync(PageableInput input);

    /// <summary>
    /// Dodaj nowe zadanie
    /// </summary>
    /// <param name="task">Zadanie</param>
    /// <returns>Nowe zadanie</returns>
    Task<Models.Task> AddAsync(Guid projectId, Models.Task task);

    /// <summary>
    /// Usuń zadanie
    /// </summary>
    /// <param name="id">Identyfikator zadania</param>
    System.Threading.Tasks.Task DeleteAsync(Guid id);

    /// <summary>
    /// Dodaj komentarz do zadania
    /// </summary>
    /// <param name="projectId">Identyfikator zadania</param>
    /// <param name="comment">Treść komentarza</param>
    /// <returns>Zadanie z dodanym komentarzem</returns>
    Task<Models.Task> AddCommentAsync(Guid taskId, Comment comment);

    /// <summary>
    /// Usuń komentarz z zadania
    /// </summary>
    /// <param name="projectId">Identyfikator zadania</param>
    /// <param name="commentId">Identyfikator komentarza</param>
    /// <returns>Zadanie z usuniętym komentarzem</returns>
    Task<Models.Task> DeleteCommentAsync(Guid taskId, Guid commentId);

    /// <summary>
    /// Zmień status zadania
    /// </summary>
    /// <param name="taskId">Identyfikator zadania</param>
    /// <param name="newStatus">Nowy status</param>
    /// <returns>Zadanie ze zmienionym statusem</returns>
    Task<Models.Task> EndTaskStatusAsync(Guid taskId);

    /// <summary>
    /// Przypisz osobę do zadania
    /// </summary>
    /// <param name="taskId">Identyfikator zadania</param>
    /// <param name="userId">Identyfikator pracownika</param>
    /// <returns>Zadanie z przypisanym pracownikiem</returns>
    Task<Models.Task> AssignPersonToTaskAsync(Guid taskId, Guid userId);

    /// <summary>
    /// Rejestruj czas poświęcony na zrobienie zadania
    /// </summary>
    /// <param name="taskId">Identyfikator zadania</param>
    /// <param name="timeInMinutes">Spędzony czas w minutach</param>
    /// <returns>Zadanie z dodanym czasem wykonania</returns>
    Task<Models.Task> RegisterTimeAsync(Guid taskId, int timeInMinutes);

    /// <summary>
    /// Zmień wybrane właściwości zadania
    /// </summary>
    /// <param name="id">Identyfikator zadania.</param>
    /// <param name="task">Dane zadania do modyfikacji</param>
    /// <returns>Zmodyfikowane zadanie</returns>
    Task<Models.Task> Patch(Guid id, Change<Models.Task> task);
  }
}
