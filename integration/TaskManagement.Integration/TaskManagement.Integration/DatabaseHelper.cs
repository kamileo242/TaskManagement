using MongoDB.Bson;
using MongoDB.Driver;

namespace TaskManagement.Integration
{
  public static class DatabaseHelper
  {
    private static IMongoDatabase database;

    public static void InitializeDatabase()
    {
      BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
      var client = new MongoClient($"mongodb://localhost:27017");
      database = client.GetDatabase("integration_test");
    }

    public static async Task ClearAllCollection()
    {
      var collectionName = await database.ListCollectionNames().ToListAsync();

      foreach (var name in collectionName)
      {
        await database.DropCollectionAsync(name);
      }
    }

    public static BsonDocument GetElementFromCollection(string collectionName)
    {
      var collection = database.GetCollection<BsonDocument>(collectionName);

      return collection.Find(new BsonDocument()).FirstOrDefault();
    }

    public static async Task AddElementToCollection<T>(string collectionName, T document)
    {
      var collection = database.GetCollection<T>(collectionName);

      await collection.InsertOneAsync(document);
    }
  }
}
