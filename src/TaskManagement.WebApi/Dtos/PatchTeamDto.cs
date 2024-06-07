namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt zespołu zespołu przy modyfikacji obiektu
  /// </summary>
  public record PatchTeamDto
  {
    /// <summary>
    /// Nazwa zespołu
    /// </summary>
    public string? Name { get; init; }
  }
}
