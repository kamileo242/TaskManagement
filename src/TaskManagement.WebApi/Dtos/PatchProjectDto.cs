namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt dto projektu przy modyfikacji obiektu
  /// </summary>
  public record PatchProjectDto
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
    /// Termin wykonania zadania
    /// </summary>
    public DateTime? Deadline { get; init; }
  }
}
