namespace Models
{
  /// <summary>
  /// Konfiguracja dostępu do bazy danych.
  /// </summary>
  public class DatabaseSetup
  {
    public const string SectionName = "Database";

    /// <summary>
    /// Ciąg konfiguracyjny połączenia z bazą danych.
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Nazwa bazy danych.
    /// </summary>
    public string DatabaseName { get; set; }
  }
}
