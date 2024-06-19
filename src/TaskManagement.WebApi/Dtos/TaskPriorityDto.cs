namespace TaskManagement.WebApi.Dtos
{
  /// <summary>
  /// Obiekt Dto priorytetu zadania
  /// </summary>
  public record TaskPriorityDto
  {
    /// <summary>
    /// Identyfikator priorytetu
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Nazwa priorytetu
    /// </summary>
    public string Name { get; init; }
  }
}
