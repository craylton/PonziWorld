using MongoDB.Driver;
using PonziWorld.Bootstrapping;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.Investors;

internal class MongoDbInvestorsRepository : MongoDbRepositoryBase<Investor>, IInvestorsRepository
{
    public MongoDbInvestorsRepository() : base("investors")
    {
    }

    public async Task AddInvestorAsync(Investor investor)
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();
        await investorsCollection.InsertOneAsync(investor);
    }

    public async Task<IEnumerable<Investor>> GetAllInvestorsAsync()
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();
        var investorsCursor = await investorsCollection.FindAsync(EmptyFilter);
        return await investorsCursor.ToListAsync();
    }

    public async Task DeleteAllInvestors()
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();
        await investorsCollection.DeleteManyAsync(EmptyFilter);
    }
}
