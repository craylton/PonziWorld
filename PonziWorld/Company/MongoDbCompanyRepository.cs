using MongoDB.Driver;
using PonziWorld.Bootstrapping;
using System.Threading.Tasks;

namespace PonziWorld.Company;

internal class MongoDbCompanyRepository : MongoDbRepositoryBase<Company>, ICompanyRepository
{
    public MongoDbCompanyRepository() : base("company")
    {
    }

    public async Task CreateNewCompanyAsync(Company company)
    {
        IMongoCollection<Company> companyCollection = GetDatabaseCollection();
        await companyCollection.DeleteManyAsync(EmptyFilter);
        await companyCollection.InsertOneAsync(company);
    }

    public async Task<Company> GetCompanyAsync()
    {
        IMongoCollection<Company> companyCollection = GetDatabaseCollection();
        var companyCursor = await companyCollection.FindAsync(EmptyFilter);
        return await companyCursor.SingleAsync();
    }

    public async Task<bool> GetCompanyExistsAsync()
    {
        IMongoCollection<Company> companyCollection = GetDatabaseCollection();
        return await companyCollection.CountDocumentsAsync(EmptyFilter) > 0;
    }
}
