using Models.Statueses;

namespace Models
{
  /// <summary>
  /// Model projektu
  /// </summary>
  public class Project
  {
    /// <summary>
    /// Identyfikator projektu
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Tytuł
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Opis
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Termin wykonania zadania
    /// </summary>
    public DateTime Deadline { get; set; }

    /// <summary>
    /// Zadania przypisane do projektu
    /// </summary>
    public List<Guid> TaskIds { get; set; }

    /// <summary>
    /// Status projektu
    /// </summary>
    public ProjectStatus Status { get; set; }

    /// <summary>
    /// Czas utworzenia projektu
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Komentarze do projektu
    /// </summary>
    public List<Comment> Comments { get; set; }
  }
}
