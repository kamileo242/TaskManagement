namespace Models
{
  /// <summary>
  /// Klasa bazowa dla modeli zmian.
  /// </summary>
  /// <typeparam name="TModel">Typ obiektu modelu.</typeparam>
  public class Change<TModel>
  {
    /// <summary>
    /// Dane obiektu do modyfikacji pól o wartościach różnych od null.
    /// </summary>
    public TModel Data { get; init; }

    /// <summary>
    /// Lista pól do modyfikacji o wartościach różnych od null.
    /// </summary>
    public IEnumerable<string> Updates { get; init; }
  }
}

