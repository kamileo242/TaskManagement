using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer.Dbos
{
  /// <summary>
  /// Model komentarza w bazie danych
  /// </summary>
  [Serializable]
  [BsonKnownTypes(typeof(CommentDbo))]
  public class CommentDbo
  {
    /// <summary>
    /// Identyfikator komentarza
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Autor komentarza
    /// </summary>
    [BsonRequired]
    [BsonElement("author")]
    public string Author { get; set; }

    /// <summary>
    /// Treść komentarza
    /// </summary>
    [BsonRequired]
    [BsonElement("content")]
    public string Content { get; set; }

    /// <summary>
    /// Czas utworzenia komentarza
    /// </summary>
    [BsonRequired]
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }
  }
}
