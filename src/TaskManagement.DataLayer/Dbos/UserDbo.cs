using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer.Dbos
{
  /// Model użytkownika w bazie danych
  /// </summary>
  [Serializable]
  [BsonKnownTypes(typeof(UserDbo))]
  public class UserDbo
  {
    /// <summary>
    /// Identyfikator użytkownika
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Imię
    /// </summary>
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    [BsonElement("name")]
    public string Name { get; set; }

    /// <summary>
    /// Nazwisko
    /// </summary>
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    [BsonElement("surname")]
    public string Surname { get; set; }

    /// <summary>
    /// Adres Email
    /// </summary>
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    [BsonElement("email")]
    public string Email { get; set; }

    /// <summary>
    /// Stanowisko
    /// </summary>
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    [BsonElement("position")]
    public string Position { get; set; }

    /// <summary>
    /// Numer telefonu
    /// </summary>
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    [BsonElement("phone_number")]
    public string PhoneNumber { get; set; }
  }
}
