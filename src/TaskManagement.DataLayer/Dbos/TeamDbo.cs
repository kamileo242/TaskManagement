using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer.Dbos
{
  /// <summary>
  /// Model zespołu w bazie danych
  /// </summary>
  [Serializable]
  [BsonKnownTypes(typeof(TeamDbo))]
  public class TeamDbo
  {
    /// <summary>
    /// Identyfikator zespołu
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Nazwa zespołu
    /// </summary>
    [BsonRequired]
    [BsonElement("name")]
    public string Name { get; set; }

    /// <summary>
    /// Lider zespołu
    /// </summary>
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    [BsonElement("team_leader")]
    public Guid? TeamLeaderId { get; set; }

    /// <summary>
    /// Lista członków zespołu
    /// </summary>
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    [BsonElement("user_ids")]
    public Guid[] UserIds { get; set; }
  }
}
