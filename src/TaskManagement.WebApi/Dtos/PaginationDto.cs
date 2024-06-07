namespace WebApi.Dtos
{
  /// <summary>
  /// Informacje o podziale na strony.
  /// </summary>
  public record PaginationDto
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
    {
      get
      {
        if (TotalElements > 0 && PageSize > 0)
        {
          return (TotalElements - 1) / PageSize + 1;
        }
        return 0;
      }
    }

    /// <summary>
    /// Pusta informacja o podziale na strony.
    /// </summary>
    public static PaginationDto Empty { get; } = new PaginationDto();

    /// <summary>
    /// Informacja o podziale na strony dla jednego elementu.
    /// </summary>
    public static PaginationDto Single { get; } = new PaginationDto { TotalElements = 1, PageSize = 1 };
  }
}
