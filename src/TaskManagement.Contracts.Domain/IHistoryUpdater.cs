using Models;

namespace Domain
{
  /// <summary>
  /// Serwis modyfikujący historię zmian
  /// </summary>
  public interface IHistoryUpdater
  {
    /// <summary>
    /// Typ modyfikowanego obiektu.
    /// </summary>
    string ObjectType { get; }

    /// <summary>
    /// Identyfikator modyfikowanego obiektu.
    /// </summary>
    string ObjectId { get; }

    /// <summary>
    /// Typ wykonywanej operacji
    /// </summary>
    string OperationMessage { get; }

    /// <summary>
    /// Lista zmian obiektu
    /// </summary>
    List<ChangeDetails> ChangeDetails { get; }

    /// <summary>
    /// Metoda do zapisu identyfikatora obiektu w formacie Guid oraz typu obiektu.
    /// </summary>
    /// <typeparam name="T">Typ zmiennej</typeparam>
    /// <param name="objectId">Identyfikator obiektu</param>
    void SetObjectId<T>(Guid? objectId);

    /// <summary>
    /// Metoda do zapisu zmian dokonanych na obiekcie
    /// </summary>
    /// <typeparam name="TEntity">Typ obiektu</typeparam>
    /// <param name="originalEntity">Obiekt wejściowy</param>
    /// <param name="newEntity">Obiekt wynikowy</param>
    void SetChangeDetails<TEntity>(TEntity originalEntity, TEntity newEntity);

    /// <summary>
    /// Metoda do zapisu zmian dokonanych na listach obiektu
    /// </summary>
    /// <typeparam name="TEntity">Typ obiektu</typeparam>
    /// <param name="originalEntity">Obiekt wejściowy</param>
    /// <param name="newEntity">Obiekt wynikowy</param>
    void SetChangeDetailsList<TEntity>(TEntity originalEntity, TEntity newEntity);

    /// <summary>
    /// Metoda do zapisu zmiany statusu obiektu
    /// </summary>
    /// <param name="oldStatus">Status wejściowy</param>  
    /// <param name="newStatus">Status wynikowy</param>
    void SetChangeDetailsStatus(string oldStatus, string newStatus);
  }
}
