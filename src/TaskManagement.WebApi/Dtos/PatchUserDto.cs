namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt dto użytkownika przy modyfikacji obiektu
  /// </summary>
  public record PatchUserDto
  {
    /// <summary>
    /// Imię
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Nazwisko
    /// </summary
    public string? Surname { get; init; }

    /// <summary>
    /// Adres Email
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Stanowisko
    /// </summary>
    public string? Position { get; init; }

    /// <summary>
    /// Numer telefonu
    /// </summary>
    public string? PhoneNumber { get; init; }
  }
}
