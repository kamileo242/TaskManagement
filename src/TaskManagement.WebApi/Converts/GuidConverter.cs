namespace WebApi.Converts
{
  /// <summary>
  /// Klasa konwertująca typy zmiennych
  /// </summary>
  public static class GuidConverter
  {
    /// <summary>
    /// Konwersja zmiennej typu Guid na zmienną typu string
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public static string GuidToText(this Guid guid)
        => guid.ToString("N");

    /// <summary>
    /// Konwersja zmiennej typu string na zmienna typu Guid
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static Guid TextToGuid(this string text)
        => Guid.ParseExact(text, "N");

    /// <summary>
    /// Konwersja zmiennej typu Guid? na zmienną typu string
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public static string GuidToTextOrNull(this Guid? guid)
        => guid?.ToString("N") ?? string.Empty;

    /// <summary>
    /// Konwersja zmiennej typu string na zmienną typu Guid?
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static Guid? TextToGuidOrNull(this string text)
        => Guid.TryParseExact(text, "N", out Guid result) ? result : (Guid?) null;
  }
}
