namespace Models
{
  /// <summary>
  /// Informacje o podziale na strony.
  /// </summary>
  public class Pagination
  {
    /// <summary>
    /// Numer strony indeksowany od zera.
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Liczba elementów na stronie.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Całkowita liczba elementów.
    /// </summary>
    public int TotalElements { get; init; }

    /// <summary>
    /// Liczba stron.
    /// </summary>
    public int TotalPages
      => TotalElements > 0 && PageSize > 0
       ? (TotalElements - 1) / PageSize + 1
       : 0;

    /// <summary>
    /// Pusta informacja o podziale na strony.
    /// </summary>
    public static Pagination Empty { get; } = new Pagination();

    /// <summary>
    /// Informacja o podziale na strony dla jednego elementu.
    /// </summary>
    public static Pagination Single { get; } = new Pagination { TotalElements = 1, PageSize = 1 };
  }
}
