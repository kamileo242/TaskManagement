namespace Models
{
  /// <summary>
  /// Szczegóły historii zmian
  /// </summary>
  public class ChangeDetails
  {
    /// <summary>
    /// Nazwa właściwości
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    /// Poprzednia wartość
    /// </summary>
    public string OldValue { get; set; }

    /// <summary>
    /// Nowa wartość
    /// </summary>
    public string NewValue { get; set; }

    public ChangeDetails(string propertyName, string oldValue, string newValue)
    {
      PropertyName = propertyName;
      OldValue = oldValue;
      NewValue = newValue;
    }
  }
}
