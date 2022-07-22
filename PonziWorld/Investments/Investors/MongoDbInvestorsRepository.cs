using MongoDB.Driver;
using PonziWorld.Bootstrapping;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.Investments.Investors;

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

    public async Task<IEnumerable<Investor>> GetAllActiveInvestorsAsync()
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();
        var sortOrder = Builders<Investor>.Sort.Descending(investor => investor.Investment);
        var filter = Builders<Investor>.Filter.Gt(investor => investor.Investment, 0);
        var findOptions = new FindOptions<Investor, Investor> { Sort = sortOrder };

        var investorsCursor = await investorsCollection.FindAsync(filter, findOptions);
        return await investorsCursor.ToListAsync();
    }

    public async Task<IEnumerable<Investor>> GetAllProspectiveInvestorsAsync()
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();
        var sortOrder = Builders<Investor>.Sort.Descending(investor => investor.Investment);
        var filter = Builders<Investor>.Filter.Eq(investor => investor.Investment, 0);

        var investorsCursor = await investorsCollection.FindAsync(filter);
        return await investorsCursor.ToListAsync();
    }

    public async Task<bool> GetInvestorExistsAsync(Investor newInvestor)
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();
        var filter = Builders<Investor>.Filter.Eq(investor => investor.Id, newInvestor.Id);
        return await investorsCollection.CountDocumentsAsync(filter) > 0;
    }

    public async Task<Investor> GetInvestorByIdAsync(Guid investorId)
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();
        var filter = Builders<Investor>.Filter.Eq(investor => investor.Id, investorId);
        var investorsCursor = await investorsCollection.FindAsync(filter);
        return await investorsCursor.SingleAsync();
    }

    public async Task ApplyInvestmentAsync(Investment investment)
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();
        var filter = Builders<Investor>.Filter.Eq(investor => investor.Id, investment.InvestorId);
        var update = Builders<Investor>.Update.Inc(investor => investor.Investment, investment.Amount);
        await investorsCollection.UpdateOneAsync(filter, update);
    }

    public async Task DeleteAllInvestors()
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();
        await investorsCollection.DeleteManyAsync(EmptyFilter);
    }
}
