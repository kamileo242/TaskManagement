using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt dto użytkownika przy dodawaniu nowego obiektu
  /// </summary>
  public record StoreUserDto
  {
    /// <summary>
    /// Imię
    /// </summary>
    [Required]
    public string Name { get; init; }

    /// <summary>
    /// Nazwisko
    /// </summary>
    [Required]
    public string Surname { get; init; }

    /// <summary>
    /// Adres Email
    /// </summary>
    [Required]
    public string Email { get; init; }

    /// <summary>
    /// Stanowisko
    /// </summary>
    [Required]
    public string Position { get; init; }

    /// <summary>
    /// Numer telefonu
    /// </summary>
    [Required]
    public string PhoneNumber { get; init; }
  }
}
