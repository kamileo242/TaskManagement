namespace Domain.Providers
{
  /// <summary>
  /// Klasa tworząca unikalny identyfikatorr typu Guid
  /// </summary>
  public static class GuidProvider
  {
    /// <summary>
    /// Metoda tworząca unikalny identyfikatorr typu Guid
    /// </summary>
    /// <returns>identyfikator typu Guid</returns>
    public static Guid GenetareGuid()
        => Guid.NewGuid();
  }
}
