namespace Models
{
  /// <summary>
  /// Model użytkownika
  /// </summary>
  public class User
  {
    /// <summary>
    /// Identyfikator użytkownika
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Imię
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Nazwisko
    /// </summary>
    public string Surname { get; set; }

    /// <summary>
    /// Adres Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Stanowisko
    /// </summary>
    public string Position { get; set; }

    /// <summary>
    /// Numer telefonu
    /// </summary>
    public string PhoneNumber { get; set; }
  }
}
