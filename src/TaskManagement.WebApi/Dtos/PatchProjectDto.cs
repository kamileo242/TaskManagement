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
    /// Priorytet projektu w skali od 1 do 5,
    /// </summary>
    public int? Priority { get; init; }

    /// <summary>
    /// Termin wykonania zadania
    /// </summary>
    public DateTime? Deadline { get; init; }
  }
}
