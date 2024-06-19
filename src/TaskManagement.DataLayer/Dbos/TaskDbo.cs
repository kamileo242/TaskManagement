using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer.Dbos
{
  /// <summary>
  /// Model zadania w bazie danych
  /// </summary>
  [Serializable]
  [BsonKnownTypes(typeof(TaskDbo))]
  public class TaskDbo
  {
    /// <summary>
    /// Identyfikator zadania
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Tytuł
    /// </summary>
    [BsonRequired]
    [BsonElement("title")]
    public string Title { get; set; }

    /// <summary>
    /// Opis
    /// </summary>
    [BsonRequired]
    [BsonElement("description")]
    public string Description { get; set; }

    /// <summary>
    /// Priorytet zadania,
    /// </summary>
    [BsonRequired]
    [BsonElement("priority")]
    public string Priority { get; set; }

    /// <summary>
    /// Termin wykonania zadania
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    [BsonElement("deadline")]
    public DateTime Deadline { get; set; }

    /// <summary>
    /// Czas spędzony na zrobieniu zadania w minutach
    /// </summary>
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    [BsonElement("spent_time")]
    public int? SpentTime { get; set; }

    /// <summary>
    /// Osoba przypisana do zadania
    /// </summary>
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    [BsonElement("assigned_person_id")]
    public Guid AssignedPersonId { get; set; }

    /// <summary>
    /// Status zadania
    /// </summary>
    [BsonRequired]
    [BsonElement("status")]
    public string Status { get; set; }

    /// <summary>
    /// Czas utworzenia zadania
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Komentarze do projektu
    /// </summary>
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    [BsonElement("comments")]
    public CommentDbo[] Comments { get; set; }
  }
}
