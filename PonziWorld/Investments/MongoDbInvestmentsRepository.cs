using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.Investments;

internal class MongoDbInvestmentsRepository : Database.MongoDbRepositoryBase<Investment>, IInvestmentsRepository
{
    public MongoDbInvestmentsRepository() : base("investments")
    {
    }

    public async Task AddInvestmentAsync(Investment investment)
    {
        IMongoCollection<Investment> investmentsCollection = GetDatabaseCollection();
        await investmentsCollection.InsertOneAsync(investment);
    }

    public async Task<List<Investment>> GetInvestmentsByMonthAsync(int month)
    {
        IMongoCollection<Investment> investmentsCollection = GetDatabaseCollection();

        FilterDefinition<Investment> filter = Builders<Investment>.Filter
            .Eq(investment => investment.Month, month);

        IAsyncCursor<Investment> cursor = await investmentsCollection.FindAsync(filter);
        return await cursor.ToListAsync();
    }

    public async Task DeleteAllInvestments()
    {
        IMongoCollection<Investment> investmentsCollection = GetDatabaseCollection();
        await investmentsCollection.DeleteManyAsync(EmptyFilter);
    }
}
