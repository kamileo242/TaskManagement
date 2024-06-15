using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;
using Task = System.Threading.Tasks.Task;

namespace DataLayer.Repositories
{
  /// <summary>
  /// Implementacja podstawowego repozytorium obiektów.
  /// </summary>
  /// <typeparam name="TEntity">Typ danych w repozytorium.</typeparam>
  /// <typeparam name="TDbo">Typ danych w bazie danych.</typeparam>
  /// <typeparam name="TId">Typ identyfikatora.</typeparam>
  public abstract class AbstractRepository<TEntity, TDbo, TId> : IRepository<TEntity, TId>
    where TEntity : class
    where TDbo : class
  {
    private readonly IDboConverter converter;
    private readonly IMongoCollection<TDbo> collection;

    public AbstractRepository(DatabaseSetup setup, IDboConverter converter, string collectionName)
    {
      this.converter = converter;
      var client = new MongoClient(setup.ConnectionString);
      var database = client.GetDatabase(setup.DatabaseName);
      collection = database.GetCollection<TDbo>(collectionName);
      BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
    }

    public async Task<TEntity> GetByIdAsync(TId id)
    {
      var filter = Builders<TDbo>.Filter.Eq("_id", id);
      var result = await collection.Find(filter).FirstOrDefaultAsync();

      return converter.Convert<TEntity>(result);
    }

    public async Task<PageableResult<TEntity>> GetAllAsync(PageableInput input)
    {
      var filter = Builders<TDbo>.Filter.Empty;
      SortDefinition<TDbo> sortDefinition = null;

      if (input.Sorting != null)
      {
        sortDefinition = Builders<TDbo>.Sort.Combine(input.Sorting
            .Select(sortPart => sortPart.IsAscending
                ? Builders<TDbo>.Sort.Ascending(sortPart.Name)
                : Builders<TDbo>.Sort.Descending(sortPart.Name)));
      }

      var result = await collection.Find(filter)
          .Sort(sortDefinition)
          .Skip(input.PageNumber * input.PageSize)
          .Limit(input.PageSize)
          .ToListAsync();

      var totalCount = await collection.CountDocumentsAsync(_ => true);

      var convertedResult = result.Select(dbo => converter.Convert<TEntity>(dbo)).ToArray();

      var pageableResult = new PageableResult<TEntity>
      {
        Items = convertedResult,
        Pagination = new Pagination
        {
          PageNumber = input.PageNumber,
          PageSize = input.PageSize,
          TotalElements = Convert.ToInt32(totalCount),
        }
      };

      return pageableResult;
    }

    public async Task<TEntity> StoreAsync(TEntity item)
    {
      var dbo = converter.Convert<TDbo>(item);

      var idProperty = typeof(TDbo).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);

      var idValue = idProperty.GetValue(dbo);

      var filter = Builders<TDbo>.Filter.Eq("_id", idValue);
      var existingItem = await collection.Find(filter).FirstOrDefaultAsync();

      if (existingItem == null)
      {
        await collection.InsertOneAsync(dbo);
      }

      return await Task.FromResult(converter.Convert<TEntity>(dbo));
    }

    public async Task<TEntity> ChangeOneAsync(TId id, Change<TEntity> change)
    {
      var filter = Builders<TDbo>.Filter.Eq("_id", id);
      var existingItem = await collection.Find(filter).FirstOrDefaultAsync();

      if (existingItem == null)
      {
        return null;
      }

      var existingItemModel = converter.Convert<TEntity>(existingItem);

      foreach (var propertyName in change.Updates)
      {
        var property = typeof(TEntity).GetProperty(propertyName);
        var newValue = property.GetValue(change.Data);

        if ((property.PropertyType == typeof(DateTime) && (DateTime) newValue == DateTime.MinValue) ||
        (property.PropertyType == typeof(int) && (int) newValue == 0))
        {
          continue;
        }

        property.SetValue(existingItemModel, newValue);
      }

      var dbo = converter.Convert<TDbo>(existingItemModel);
      await collection.ReplaceOneAsync(filter, dbo);

      return await Task.FromResult(converter.Convert<TEntity>(dbo));
    }

    public async Task RemoveAsync(TId id)
    {
      var filter = Builders<TDbo>.Filter.Eq("_id", id);
      await collection.DeleteOneAsync(filter);
    }
  }
}
