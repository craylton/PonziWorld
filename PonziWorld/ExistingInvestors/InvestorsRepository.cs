using MongoDB.Driver;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace PonziWorld.ExistingInvestors;

internal class InvestorsRepository : IInvestorsRepository
{
    private readonly ConnectionStringSettings settings;

    public InvestorsRepository() =>
        settings = ConfigurationManager.ConnectionStrings["MainDatabase"];

    public async Task<IEnumerable<ExistingInvestor>> GetAllInvestorsAsync()
    {
        IMongoCollection<ExistingInvestor> investorsCollection = GetDatabaseInvestorsCollection();
        var filter = FilterDefinition<ExistingInvestor>.Empty;
        var investorsCursor = await investorsCollection.FindAsync(filter);
        return await investorsCursor.ToListAsync();
    }

    public async Task AddInvestorAsync(ExistingInvestor investor)
    {
        IMongoCollection<ExistingInvestor> investorsCollection = GetDatabaseInvestorsCollection();
        await investorsCollection.InsertOneAsync(investor);
    }

    private IMongoCollection<ExistingInvestor> GetDatabaseInvestorsCollection()
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase("ponziworld");
        return database.GetCollection<ExistingInvestor>("investors");
    }
}
