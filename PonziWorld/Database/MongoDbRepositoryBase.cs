using MongoDB.Driver;
using System.Configuration;

namespace PonziWorld.Database;

internal abstract class MongoDbRepositoryBase<TEntity>
{
    private readonly ConnectionStringSettings settings;

    protected const string DatabaseName = "ponziworld";

    protected string CollectionName { get; }

    protected static FilterDefinition<TEntity> EmptyFilter =>
        FilterDefinition<TEntity>.Empty;

    public MongoDbRepositoryBase(string collectionName)
    {
        settings = ConfigurationManager.ConnectionStrings["MainDatabase"];
        CollectionName = collectionName;
    }

    protected IMongoCollection<TEntity> GetDatabaseCollection()
    {
        MongoClient client = new(settings.ConnectionString);
        IMongoDatabase database = client.GetDatabase(DatabaseName);
        return database.GetCollection<TEntity>(CollectionName);
    }
}
