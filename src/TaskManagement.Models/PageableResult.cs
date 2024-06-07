namespace Models
{
  /// <summary>
  /// Klasa reprezentująca listę obiektów z podziałem na strony.
  /// </summary>
  /// <typeparam name="T">Typ danych wynikowych.</typeparam>
  public class PageableResult<T>
  {
    /// <summary>
    /// Kolekcja danych wynikowych znajdujących się na stronie.
    /// </summary>
    public T[] Items { get; set; }

    /// <summary>
    /// Informacja o stronie z wynikiem wyszukiwania.
    /// </summary>
    public Pagination Pagination { get; set; }
  }
}
