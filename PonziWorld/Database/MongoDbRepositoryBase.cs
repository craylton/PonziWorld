using MongoDB.Driver;
using System.Configuration;

namespace PonziWorld.Bootstrapping;

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
        var client = new MongoClient(settings.ConnectionString);
        IMongoDatabase database = client.GetDatabase(DatabaseName);
        return database.GetCollection<TEntity>(CollectionName);
    }
}
