namespace WebApi.Dtos
{
  /// <summary>
  /// Szczegóły historii zmian obiektu dto
  /// </summary>
  public record ChangeDetailsDto
  {
    /// <summary>
    /// Nazwa właściwości
    /// </summary>
    public string PropertyName { get; init; }

    /// <summary>
    /// Poprzednia wartość
    /// </summary>
    public string OldValue { get; init; }

    /// <summary>
    /// Nowa wartość
    /// </summary>
    public string NewValue { get; init; }
  }
}
