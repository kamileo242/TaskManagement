using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt zespołu zespołu przy dodawaniu nowego obiektu
  /// </summary>
  public record StoreTeamDto
  {
    /// <summary>
    /// Nazwa zespołu
    /// </summary>
    [Required]
    public string Name { get; init; }
  }
}
