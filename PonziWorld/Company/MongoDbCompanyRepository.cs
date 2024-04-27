using MongoDB.Driver;
using PonziWorld.Core;
using System.Threading.Tasks;

namespace PonziWorld.Company;

internal class MongoDbCompanyRepository()
    : MongoDbRepositoryBase<Company>("company"), ICompanyRepository
{
    public async Task CreateNewCompanyAsync(Company company)
    {
        IMongoCollection<Company> companyCollection = GetDatabaseCollection();
        await companyCollection.DeleteManyAsync(EmptyFilter);
        await companyCollection.InsertOneAsync(company);
    }

    public async Task<Company> GetCompanyAsync()
    {
        IMongoCollection<Company> companyCollection = GetDatabaseCollection();
        IAsyncCursor<Company> companyCursor = await companyCollection.FindAsync(EmptyFilter);
        return await companyCursor.SingleAsync();
    }

    public async Task<bool> GetCompanyExistsAsync()
    {
        IMongoCollection<Company> companyCollection = GetDatabaseCollection();
        return await companyCollection.CountDocumentsAsync(EmptyFilter) > 0;
    }

    public async Task MoveToNextMonthAsync(double totalChangeInFunds)
    {
        IMongoCollection<Company> companyCollection = GetDatabaseCollection();

        UpdateDefinition<Company> update = Builders<Company>.Update
            .Inc(company => company.ActualFunds, totalChangeInFunds)
            .Inc(company => company.ClaimedFunds, totalChangeInFunds)
            .Inc(company => company.Month, 1);

        await companyCollection.UpdateOneAsync(EmptyFilter, update);
    }

    public async Task AddProfitAsync(double profit)
    {
        IMongoCollection<Company> companyCollection = GetDatabaseCollection();

        UpdateDefinition<Company> update = Builders<Company>.Update
            .Inc(company => company.ActualFunds, profit);

        await companyCollection.UpdateOneAsync(EmptyFilter, update);
    }

    public async Task ClaimInterest(double claimedInterestRate)
    {
        IMongoCollection<Company> companyCollection = GetDatabaseCollection();

        UpdateDefinition<Company> update = Builders<Company>.Update
            .Mul(company => company.ClaimedFunds, claimedInterestRate);

        await companyCollection.UpdateOneAsync(EmptyFilter, update);
    }
}
