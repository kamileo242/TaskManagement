namespace Domain.Providers
{
  /// <summary>
  /// Klasa generująca aktualny czas
  /// </summary>
  public static class TimeProvider
  {
    /// <summary>
    /// Metoda generująca aktualny czas
    /// </summary>
    /// <returns>Aktualny czas</returns>
    public static DateTime GetTime()
        => DateTime.Now;
  }
}
