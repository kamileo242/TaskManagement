namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt zespołu zespołu
  /// </summary>
  public record TeamDto
  {
    /// <summary>
    /// Identyfikator zespołu
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Nazwa zespołu
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Identyfikator lidera zespołu
    /// </summary>
    public UserDto TeamLeader { get; init; }

    /// <summary>
    /// Lista identyfikatorów członków zespołu
    /// </summary>
    public IEnumerable<UserDto> Users { get; init; }
  }
}
