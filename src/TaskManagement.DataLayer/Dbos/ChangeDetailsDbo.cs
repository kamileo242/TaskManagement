using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer.Dbos
{
  /// <summary>
  /// Model szczegółów historii zmian w bazie danych
  /// </summary>
  [Serializable]
  [BsonKnownTypes(typeof(ChangeDetailsDbo))]
  public class ChangeDetailsDbo
  {
    /// <summary>
    /// Nazwa właściwości
    /// </summary>
    [BsonRequired]
    [BsonElement("property_name")]
    public string PropertyName { get; set; }

    /// <summary>
    /// Poprzednia wartość
    /// </summary>
    [BsonIgnoreIfNull]
    [BsonElement("old_value")]
    public string OldValue { get; set; }

    /// <summary>
    /// Nowa wartość
    /// </summary>

    [BsonIgnoreIfNull]
    [BsonElement("new_value")]
    public string NewValue { get; set; }
  }
}
