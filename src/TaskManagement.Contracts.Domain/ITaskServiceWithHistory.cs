using Models;

namespace Domain
{
  /// <summary>
  /// Zbiór operacji zapisujących historię zmian dla operacji wykonywanych na zadaniach
  /// </summary>
  public interface ITaskServiceWithHistory
  {
    /// <summary>
    /// Pobierz zadanie po id
    /// </summary>
    /// <param name="id">Identyfikator zadania</param>
    /// <returns>Zadanie</returns>
    Task<Models.Task> GetByIdAsync(Guid id);

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
    Task<Models.Task> AddAsync(OperationContext context, Guid projectId, Models.Task task);

    /// <summary>
    /// Usuń zadanie
    /// </summary>
    /// <param name="id">Identyfikator zadania</param>
    System.Threading.Tasks.Task DeleteAsync(OperationContext context, Guid id);

    /// <summary>
    /// Dodaj komentarz do zadania
    /// </summary>
    /// <param name="projectId">Identyfikator zadania</param>
    /// <param name="comment">Treść komentarza</param>
    /// <returns>Zadanie z dodanym komentarzem</returns>
    Task<Models.Task> AddCommentAsync(OperationContext context, Guid taskId, Comment comment);

    /// <summary>
    /// Usuń komentarz z zadania
    /// </summary>
    /// <param name="projectId">Identyfikator zadania</param>
    /// <param name="commentId">Identyfikator komentarza</param>
    /// <returns>Zadanie z usuniętym komentarzem</returns>
    Task<Models.Task> DeleteCommentAsync(OperationContext context, Guid taskId, Guid commentId);

    /// <summary>
    /// Zmień status zadania
    /// </summary>
    /// <param name="taskId">Identyfikator zadania</param>
    /// <param name="newStatus">Nowy status</param>
    /// <returns>Zadanie ze zmienionym statusem</returns>
    Task<Models.Task> EndTaskStatusAsync(OperationContext context, Guid taskId);

    /// <summary>
    /// Przypisz osobę do zadania
    /// </summary>
    /// <param name="taskId">Identyfikator zadania</param>
    /// <param name="userId">Identyfikator pracownika</param>
    /// <returns>Zadanie z przypisanym pracownikiem</returns>
    Task<Models.Task> AssignPersonToTaskAsync(OperationContext context, Guid taskId, Guid userId);

    /// <summary>
    /// Rejestruj czas poświęcony na zrobienie zadania
    /// </summary>
    /// <param name="taskId">Identyfikator zadania</param>
    /// <param name="timeInMinutes">Spędzony czas w minutach</param>
    /// <returns>Zadanie z dodanym czasem wykonania</returns>
    Task<Models.Task> RegisterTimeAsync(OperationContext context, Guid taskId, int timeInMinutes);

    /// <summary>
    /// Zmień wybrane właściwości zadania
    /// </summary>
    /// <param name="id">Identyfikator zadania.</param>
    /// <param name="task">Dane zadania do modyfikacji</param>
    /// <returns>Zmodyfikowane zadanie</returns>
    Task<Models.Task> PatchAsync(OperationContext context, Guid id, Change<Models.Task> task);
  }
}
