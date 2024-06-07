namespace Models
{
  /// <summary>
  /// Model zadania
  /// </summary>
  public class Task
  {
    /// <summary>
    /// Identyfikator zadania
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
    /// Priorytet zadania w skali od 0 do 5,
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Termin wykonania zadania
    /// </summary>
    public DateTime Deadline { get; set; }

    /// <summary>
    /// Czas spędzony na zrobieniu zadania w minutach
    /// </summary>
    public int SpentTime { get; set; }

    /// <summary>
    /// Osoba przypisana do zadania
    /// </summary>
    public Guid AssignedPersonId { get; set; }

    /// <summary>
    /// Status zadania
    /// </summary>
    public Models.Statueses.TaskStatus Status { get; set; }

    /// <summary>
    /// Czas utworzenia zadania
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Komentarze do projektu
    /// </summary>
    public List<Comment> Comments { get; set; }
  }
}
