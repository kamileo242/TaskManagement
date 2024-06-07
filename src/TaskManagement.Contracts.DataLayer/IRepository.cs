using Models;
using Task = System.Threading.Tasks.Task;

namespace DataLayer
{
  /// <summary>
  /// Zbiór operacji wykonywanych na repozytorium obiektów.
  /// </summary>
  /// <typeparam name="TEntity">Typ obiektów w repozytorium.</typeparam>
  /// <typeparam name="TId">Typ identyfikatora obiektów.</typeparam>
  public interface IRepository<TEntity, in TId>
  {
    /// <summary>
    /// Zwraca obiekt o podanym identyfikatorze.
    /// </summary>
    /// <param name="id">Identyfikator obiektu do znalezienia.</param>
    /// <returns>Znaleziony obiekt.</returns>
    Task<TEntity> GetByIdAsync(TId id);

    /// <summary>
    /// Zwraca kolekcję wszystkich obiektów.
    /// </summary>
    /// <returns>Kolekcja wszystkich obiektów.</returns>
    Task<PageableResult<TEntity>> GetAllAsync(PageableInput input);

    /// <summary>
    /// Dodanie lub zmiana danych obiektu w repozytorium.
    /// </summary>
    /// <param name="entity">Obiekt do modyfikacji.</param>
    /// <returns>Obiekt po modyfikacji.</returns>
    Task<TEntity> StoreAsync(TEntity entity);

    /// <summary>
    /// Modyfikacja wybranych właściwości obiektu.
    /// </summary>
    /// <param name="entity">Obiekt do modyfikacji.</param>
    /// <param name="id">Identyfikator obiektu do znalezienia.</param>
    /// <returns>Obiekt po modyfikacji.</returns>
    Task<TEntity> ChangeOneAsync(TId id, Change<TEntity> entity);

    /// <summary>
    /// Usunięcie obiektu o podanym identyfikatorze.
    /// </summary>
    /// <param name="id">Identyfikator obiektu do usunięcia.</param>
    Task RemoveAsync(TId id);
  }
}
