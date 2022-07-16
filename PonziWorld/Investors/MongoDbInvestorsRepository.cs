using MongoDB.Driver;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace PonziWorld.Investors;

internal class MongoDbInvestorsRepository : IInvestorsRepository
{
    private readonly ConnectionStringSettings settings;

    public MongoDbInvestorsRepository() =>
        settings = ConfigurationManager.ConnectionStrings["MainDatabase"];

    public async Task AddInvestorAsync(Investor investor)
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseInvestorsCollection();
        await investorsCollection.InsertOneAsync(investor);
    }

    public async Task<IEnumerable<Investor>> GetAllInvestorsAsync()
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseInvestorsCollection();
        var filter = FilterDefinition<Investor>.Empty;
        var investorsCursor = await investorsCollection.FindAsync(filter);
        return await investorsCursor.ToListAsync();
    }

    public async Task DeleteAllInvestors()
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseInvestorsCollection();
        var filter = FilterDefinition<Investor>.Empty;
        await investorsCollection.DeleteManyAsync(filter);
    }

    private IMongoCollection<Investor> GetDatabaseInvestorsCollection()
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase("ponziworld");
        return database.GetCollection<Investor>("investors");
    }
}
