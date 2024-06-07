namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt dto historii zmian
  /// </summary>
  public record DataChangeDto
  {
    /// <summary>
    /// Identyfikator historii zmian
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Czas wykonania operacji
    /// </summary>
    public DateTime OperationTime { get; init; }

    /// <summary>
    /// Typ zmodyfikowanego obiektu
    /// </summary>
    public string ObjectType { get; init; }

    /// <summary>
    /// Identyfikator zmodyfikowanego obiektu
    /// </summary>
    public string ObjectId { get; init; }

    /// <summary>
    /// Typ operacji
    /// </summary>
    public string OperationType { get; init; }

    /// <summary>
    /// Rezultat operacji (powodzenie lub nie)
    /// </summary>
    public bool OperationResult { get; init; }

    /// <summary>
    /// Szczegóły modyfikacji
    /// </summary>
    public IEnumerable<ChangeDetailsDto> ChangeDetails { get; init; }
  }
}
