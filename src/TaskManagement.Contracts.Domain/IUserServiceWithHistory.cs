using Models;
using Task = System.Threading.Tasks.Task;

namespace Domain
{
  /// <summary>
  /// Zbiór operacji zapisujących historię zmian dla operacji wykonywanych na użytkownikach
  /// </summary>
  public interface IUserServiceWithHistory
  {
    /// <summary>
    /// Pobierz użytkownika po id
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    /// <returns>Użytkownik</returns>
    Task<User> GetByIdAsync(Guid id);

    /// <summary>
    /// Pobierz wszytskich użytkowników
    /// </summary>
    /// <param name="input">Parametry paginacji</param>
    /// <returns>Lista użytkowników</returns>
    Task<PageableResult<User>> GetAllAsync(PageableInput input);

    /// <summary>
    /// Dodaj nowego użytkownika
    /// </summary>
    /// <param name="user">Użytkownik</param>
    /// <returns>Nowy użytkownik</returns>
    Task<User> AddAsync(OperationContext context, User user);

    /// <summary>
    /// Usuń użytkownika
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    Task DeleteAsync(OperationContext context, Guid id);

    /// <summary>
    /// Zmień wybrane właściwości użytkownika
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    /// <param name="user">Dane użytkownika do modyfikacji</param>
    /// <returns>Zmodyfikowany użytkownik</returns>
    Task<User> PatchAsync(OperationContext context, Guid id, Change<User> user);
  }
}
