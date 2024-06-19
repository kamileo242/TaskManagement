namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt dto zadania przy modyfikacji obiektu
  /// </summary>
  public record PatchTaskDto
  {
    /// <summary>
    /// Tytuł
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Opis
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Priorytet zadania,
    /// </summary>
    public string Priority { get; init; }

    /// <summary>
    /// Termin wykonania zadania
    /// </summary>
    public DateTime? Deadline { get; init; }
  }
}
