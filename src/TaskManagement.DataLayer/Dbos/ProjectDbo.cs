using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer.Dbos
{
  /// <summary>
  /// Model projektu w bazie danych
  /// </summary>
  [Serializable]
  [BsonKnownTypes(typeof(ProjectDbo))]
  public class ProjectDbo
  {
    /// <summary>
    /// Identyfikator projektu
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
    /// Priorytet projektu w skali od 1 do 5,
    /// </summary>
    [BsonRequired]
    [BsonElement("priority")]
    public int Priority { get; set; }

    /// <summary>
    /// Termin wykonania zadania
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    [BsonRequired]
    [BsonElement("deadline")]
    public DateTime Deadline { get; set; }

    /// <summary>
    /// Zadania przypisane do projektu
    /// </summary>
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    [BsonElement("task_ids")]
    public Guid[] TasksIds { get; set; }

    /// <summary>
    /// Status projektu
    /// </summary>
    [BsonRequired]
    [BsonElement("status")]
    public string Status { get; set; }

    /// <summary>
    /// Czas utworzenia projektu
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    [BsonRequired]
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
