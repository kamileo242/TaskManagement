namespace Models
{
  /// <summary>
  /// Historia zmian
  /// </summary>
  public class DataChange
  {
    /// <summary>
    /// Identyfikator historii zmian
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Czas wykonania operacji
    /// </summary>
    public DateTime OperationTime { get; set; }

    /// <summary>
    /// Typ zmodyfikowanego obiektu
    /// </summary>
    public string ObjectType { get; set; }

    /// <summary>
    /// Identyfikator zmodyfikowanego obiektu
    /// </summary>
    public string ObjectId { get; set; }

    /// <summary>
    /// Typ operacji
    /// </summary>
    public string OperationType { get; set; }

    /// <summary>
    /// Rezultat operacji (powodzenie lub nie)
    /// </summary>
    public bool OperationResult { get; set; }

    /// <summary>
    /// Szczegóły modyfikacji
    /// </summary>
    public List<ChangeDetails> ChangeDetails { get; set; }
  }
}
