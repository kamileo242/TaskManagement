namespace Models
{
  /// <summary>
  /// Element listy parametrów sortowania.
  /// </summary>
  public struct SortPart
  {
    /// <summary>
    /// Nazwa elementu, wg którego następuje sortowanie.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Czy kolejność sortowania jest rosnąca.
    /// </summary>
    public bool IsAscending { get; init; }

    /// <summary>
    /// Utworzenie elementu sortowania z kolejnością rosnącą.
    /// </summary>
    /// <param name="name">Nazwa elementu sortowania.</param>
    /// <returns>Utworzony obiekt.</returns>
    public static SortPart Asc(string name) => new() { Name = name, IsAscending = true };

    /// <summary>
    /// Utworzenie elementu sortowania z kolejnością malejącą.
    /// </summary>
    /// <param name="name">Nazwa elementu sortowania.</param>
    /// <returns>Utworzony obiekt.</returns>
    public static SortPart Desc(string name) => new() { Name = name };
  }
}
