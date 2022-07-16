using MongoDB.Driver;
using System.Configuration;
using System.Threading.Tasks;

namespace PonziWorld.Company;

internal class MongoDbCompanyRepository : ICompanyRepository
{
    private readonly ConnectionStringSettings settings;

    public MongoDbCompanyRepository() =>
        settings = ConfigurationManager.ConnectionStrings["MainDatabase"];

    public async Task CreateNewCompanyAsync(Company company)
    {
        IMongoCollection<Company> companyCollection = GetDatabaseCompanyCollection();
        var filter = FilterDefinition<Company>.Empty;
        await companyCollection.DeleteManyAsync(filter);
        await companyCollection.InsertOneAsync(company);
    }

    public async Task<Company> GetCompanyAsync()
    {
        IMongoCollection<Company> companyCollection = GetDatabaseCompanyCollection();
        var filter = FilterDefinition<Company>.Empty;
        var companyCursor = await companyCollection.FindAsync(filter);
        return await companyCursor.SingleAsync();
    }

    public async Task<bool> GetCompanyExistsAsync()
    {
        IMongoCollection<Company> companyCollection = GetDatabaseCompanyCollection();
        var filter = FilterDefinition<Company>.Empty;
        return await companyCollection.CountDocumentsAsync(filter) > 0;
    }

    private IMongoCollection<Company> GetDatabaseCompanyCollection()
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase("ponziworld");
        return database.GetCollection<Company>("company");
    }
}
