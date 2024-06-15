namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt dto projektu
  /// </summary>
  public record ProjectDto
  {
    /// <summary>
    /// Identyfikator projektu
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
    /// Termin wykonania zadania
    /// </summary>
    public DateTime? Deadline { get; init; }

    /// <summary>
    /// Identyfikatory zadań  przypisanych do projektu
    /// </summary>
    public IEnumerable<TaskDto>? Tasks { get; init; }

    /// <summary>
    /// Status projektu
    /// </summary>
    public string Status { get; init; }

    /// <summary>
    /// Czas utworzenia projektu
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Komentarze do projektu
    /// </summary>
    public IEnumerable<CommentDto>? Comments { get; init; }
  }
}
