﻿using MongoDB.Driver;
using PonziWorld.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.Investments.Investors;

internal class MongoDbInvestorsRepository()
    : MongoDbRepositoryBase<Investor>("investors"), IInvestorsRepository
{
    public async Task AddInvestorAsync(Investor investor) => await AddOneAsync(investor);

    public async Task<IEnumerable<Investor>> GetAllInvestorsAsync() => await GetAllAsync();

    public async Task<IEnumerable<Investor>> GetAllActiveInvestorsAsync()
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();

        SortDefinition<Investor> sortOrder = Builders<Investor>.Sort
            .Descending(investor => investor.Investment);

        FilterDefinition<Investor> filter = Builders<Investor>.Filter
            .Gt(investor => investor.Investment, 0);

        FindOptions<Investor, Investor> findOptions = new() { Sort = sortOrder };

        IAsyncCursor<Investor> cursor = await investorsCollection.FindAsync(filter, findOptions);
        return await cursor.ToListAsync();
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

    public async Task ApplyInterestRateAsync(double interestRate)
    {
        IMongoCollection<Investor> investorsCollection = GetDatabaseCollection();

        var interestRateMultiplier = (interestRate / 100d) + 1;
        UpdateDefinition<Investor> update = Builders<Investor>.Update
            .Mul(investor => investor.Investment, interestRateMultiplier);

        await investorsCollection.UpdateManyAsync(EmptyFilter, update);
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

    public async Task DeleteAllInvestorsAsync() => await DeleteAllAsync();
}
