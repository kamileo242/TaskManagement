using Models;

namespace Domain
{
  /// <summary>
  /// Zbiór operacji wykonywanych na serwisie historii zmian
  /// </summary>s
  public interface IDataChangeService
  {
    /// <summary>
    /// Pobierz zmianę po id
    /// </summary>
    /// <param name="id">Identyfikator spotkania</param>
    /// <returns>Spotkanie</returns>
    Task<DataChange> GetByIdAsync(Guid id);

    /// <summary>
    /// Pobierz wszystkie zmiany wraz z paginacją
    /// </summary>
    /// <param name="input">Dane podziału na strony</param>
    /// <returns>Historia zmian wraz z paginacją</returns>
    Task<PageableResult<DataChange>> GetAllAsync(PageableInput input);
  }
}
