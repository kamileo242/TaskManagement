using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt Dto komentarzy przy dodawaniu nowego obiektu
  /// </summary>
  public record StoreCommentDto
  {
    /// <summary>
    /// Autor komentarza
    /// </summary>
    public string? Author { get; init; }

    /// <summary>
    /// Treść komentarza
    /// </summary>
    [Required]
    public string Content { get; init; }
  }
}
