using MongoDB.Driver;

namespace RepositoryTests
{
  public static class DatabaseHelper
  {
    public static async Task<T> GetElementFromCollection<T>(IMongoCollection<T> collection, object id)
    {
      var filter = Builders<T>.Filter.Eq("_id", id);
      return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public static async Task AddElementToCollection<T>(IMongoCollection<T> collection, T element)
    {
      await collection.InsertOneAsync(element);
    }
  }
}
