namespace WebApi.Dtos
{
  /// <summary>
  /// Klasa reprezentująca listę obiektów z podziałem na strony.
  /// </summary>
  /// <typeparam name="TDto">Typ danych wynikowych.</typeparam>
  public class PageableResultDto<TDto>
  {
    /// <summary>
    /// Kolekcja danych wynikowych znajdujących się na stronie.
    /// </summary>
    public TDto[] Items { get; init; }

    /// <summary>
    /// Informacja o stronie z wynikiem wyszukiwania.
    /// </summary>
    public PaginationDto Pagination { get; init; }
  }
}
