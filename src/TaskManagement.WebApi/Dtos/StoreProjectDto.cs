﻿using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
  /// <summary>
  /// Obiekt dto projektu przy dodawaniu nowego obiektu
  /// </summary>
  public record StoreProjectDto
  {
    /// <summary>
    /// Tytuł
    /// </summary>
    [Required]
    public string Title { get; init; }

    /// <summary>
    /// Opis
    /// </summary>
    [Required]
    public string Description { get; init; }

    /// <summary>
    /// Priorytet projektu w skali od 1 do 5,
    /// </summary>
    [Required]
    public int? Priority { get; init; }

    /// <summary>
    /// Termin wykonania zadania
    /// </summary>
    public DateTime? Deadline { get; init; }
  }
}
