namespace Models
{
  /// <summary>
  /// Klasa wyszukiwania danych z uwzględnieniem sortowania i podziału na strony.
  /// </summary>
  public class PageableInput
  {
    /// <summary>
    /// Numer strony indeksowany od zera.
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Oczekiwana liczba elementów na stronie.
    /// </summary>
    public int PageSize { get; init; } = int.MaxValue;

    /// <summary>
    /// Oczekiwane sortowanie wyniku wyszukiwania.
    /// </summary>
    public SortPart[] Sorting { get; init; }
  }
}
