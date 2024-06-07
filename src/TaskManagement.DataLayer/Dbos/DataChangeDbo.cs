using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer.Dbos
{
  /// <summary>
  /// Model historii zmian w bazie danych
  /// </summary>
  [Serializable]
  [BsonKnownTypes(typeof(DataChangeDbo))]
  public class DataChangeDbo
  {
    /// <summary>
    /// Identyfikator historii zmian
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Czas wykonania operacji
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    [BsonElement("operation_time")]
    public DateTime OperationTime { get; set; }

    /// <summary>
    /// Typ zmodyfikowanego obiektu
    /// </summary>
    [BsonRequired]
    [BsonElement("object_type")]
    public string ObjectType { get; set; }

    /// <summary>
    /// Identyfikator zmodyfikowanego obiektu
    /// </summary>
    [BsonRequired]
    [BsonElement("object_id")]
    public string ObjectId { get; set; }

    /// <summary>
    /// Typ operacji
    /// </summary>
    [BsonRequired]
    [BsonElement("operation_type")]
    public string OperationType { get; set; }

    /// <summary>
    /// Rezultat operacji (powodzenie lub nie)
    /// </summary>
    [BsonRequired]
    [BsonElement("operation_result")]
    public bool OperationResult { get; set; }

    /// <summary>
    /// Szczegóły modyfikacji
    /// </summary>
    [BsonRequired]
    [BsonElement("change_details")]
    public ChangeDetailsDbo[] ChangeDetails { get; set; }
  }
}
