namespace Models
{
  /// <summary>
  /// Kontekst wykonywanej operacji.
  /// </summary>
  public sealed class OperationContext
  {
    /// <summary>
    /// Typ operacji. 
    /// </summary>
    public string OperationType { get; init; }


    public OperationContext()
    {
    }

    public static OperationContext WithOperationType(
      string operationType)
      => new()
      {
        OperationType = operationType,
      };
  }
}
