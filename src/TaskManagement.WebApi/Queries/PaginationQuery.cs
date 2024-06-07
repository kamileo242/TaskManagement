using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Queries
{
  /// <summary>
  /// Klasa reprezentująca zapytanie paginacji, zawierająca informacje o numerze strony, rozmiarze strony i kryteriach sortowania.
  /// </summary>
  public class PaginationQuery
  {
    /// <summary>
    /// Numer żądanej strony wyników, indeksowany od zera.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Numer strony musi być liczbą nieujemną.")]
    [DefaultValue(0)]
    public int PageNumber { get; set; }

    /// <summary>
    /// Rozmiar żądanej strony wyników.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Rozmiar strony musi być liczbą dodanią")]
    [DefaultValue(50)]
    public int PageSize { get; set; }

    /// <summary>
    /// Lista kryteriów sortowania wyników.
    /// </summary>
    [RegularExpression(@"^(name|count),(asc|desc)(\s+(name|count),(asc|desc))*$", ErrorMessage = "Nieprawidłowa składnia sortowania.")]
    [SwaggerParameter("Sortowanie: np. \"name,asc count,desc\" lub \"name,desc count,desc\".")]
    [DefaultValue("name,asc")]
    public string Sort { get; set; }
  }
}
