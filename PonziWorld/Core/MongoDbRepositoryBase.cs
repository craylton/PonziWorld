using MongoDB.Driver;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace PonziWorld.Core;

internal abstract class MongoDbRepositoryBase<TEntity>(string collectionName)
{
    private readonly ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["MainDatabase"];

    protected const string DatabaseName = "ponziworld";

    protected string CollectionName { get; } = collectionName;

    protected static FilterDefinition<TEntity> EmptyFilter =>
        FilterDefinition<TEntity>.Empty;

    protected IMongoCollection<TEntity> GetDatabaseCollection()
    {
        MongoClient client = new(settings.ConnectionString);
        IMongoDatabase database = client.GetDatabase(DatabaseName);
        return database.GetCollection<TEntity>(CollectionName);
    }

    protected async Task AddOneAsync(TEntity entity)
    {
        IMongoCollection<TEntity> collection = GetDatabaseCollection();
        await collection.InsertOneAsync(entity);
    }

    protected async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        IMongoCollection<TEntity> collection = GetDatabaseCollection();
        IAsyncCursor<TEntity> cursor = await collection.FindAsync(EmptyFilter);
        return await cursor.ToListAsync();
    }

    protected async Task DeleteAllAsync()
    {
        IMongoCollection<TEntity> collection = GetDatabaseCollection();
        await collection.DeleteManyAsync(EmptyFilter);
    }
}
