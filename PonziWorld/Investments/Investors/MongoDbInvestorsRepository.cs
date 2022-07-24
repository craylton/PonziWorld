using MongoDB.Driver;
using PonziWorld.Database;
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
        IAsyncCursor<Investor> cursor = await investorsCollection.FindAsync(EmptyFilter);
        return await cursor.ToListAsync();
    }

    public async Task<IEnumerable<Investor>> GetAllActiveInvestorsAsync()
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();

        SortDefinition<Investor> sortOrder = Builders<Investor>.Sort
            .Descending(investor => investor.Investment);

        FilterDefinition<Investor> filter = Builders<Investor>.Filter
            .Gt(investor => investor.Investment, 0);

        FindOptions<Investor, Investor> findOptions = new() { Sort = sortOrder };

        IAsyncCursor<Investor> cursor = await investorsCollection.FindAsync(filter, findOptions);
        return  await cursor.ToListAsync();
    }

    public async Task<IEnumerable<Investor>> GetAllProspectiveInvestorsAsync()
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();

        SortDefinition<Investor> sortOrder = Builders<Investor>.Sort
            .Descending(investor => investor.Investment);

        FilterDefinition<Investor> filter = Builders<Investor>.Filter
            .Eq(investor => investor.Investment, 0);

        IAsyncCursor<Investor> cursor = await investorsCollection.FindAsync(filter);
        return await cursor.ToListAsync();
    }

    public async Task<bool> GetInvestorExistsAsync(Investor newInvestor)
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();

        FilterDefinition<Investor> filter = Builders<Investor>.Filter
            .Eq(investor => investor.Id, newInvestor.Id);

        return await investorsCollection.CountDocumentsAsync(filter) > 0;
    }

    public async Task<Investor> GetInvestorByIdAsync(Guid investorId)
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();

        FilterDefinition<Investor> filter = Builders<Investor>.Filter
            .Eq(investor => investor.Id, investorId);

        IAsyncCursor<Investor> cursor = await investorsCollection.FindAsync(filter);
        return await cursor.SingleAsync();
    }

    public async Task ApplyInvestmentAsync(Investment investment)
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();

        FilterDefinition<Investor> filter = Builders<Investor>.Filter
            .Eq(investor => investor.Id, investment.InvestorId);

        UpdateDefinition<Investor> update = Builders<Investor>.Update
            .Inc(investor => investor.Investment, investment.Amount);

        await investorsCollection.UpdateOneAsync(filter, update);
    }

    public async Task DeleteAllInvestors()
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();
        await investorsCollection.DeleteManyAsync(EmptyFilter);
    }
}
