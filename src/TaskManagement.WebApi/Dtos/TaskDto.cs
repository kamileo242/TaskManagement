namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt dto zadania
  /// </summary>
  public record TaskDto
  {
    /// <summary>
    /// Identyfikator zadania
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Tytuł
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Opis
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Priorytet zadania w skali od 0 do 5,
    /// </summary>
    public int Priority { get; init; }

    /// <summary>
    /// Termin wykonania zadania
    /// </summary>
    public DateTime? Deadline { get; init; }

    /// <summary>
    /// Czas spędzony na zrobieniu zadania
    /// </summary>
    public string SpentTime { get; init; }

    /// <summary>
    /// Osoba przypisana do zadania
    /// </summary>
    public UserDto? AssignedPerson { get; init; }

    /// <summary>
    /// Status zadania
    /// </summary>
    public string Status { get; init; }

    /// <summary>
    /// Czas utworzenia zadania
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Komentarze do projektu
    /// </summary>
    public IEnumerable<CommentDto>? Comments { get; init; }
  }
}
