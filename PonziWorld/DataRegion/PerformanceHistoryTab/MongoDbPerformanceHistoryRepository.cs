using MongoDB.Driver;
using PonziWorld.Core;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.PerformanceHistoryTab;

internal class MongoDbPerformanceHistoryRepository : MongoDbRepositoryBase<MonthlyPerformance>, IPerformanceHistoryRepository
{
    public MongoDbPerformanceHistoryRepository() : base("companyPerformance")
    {
    }

    public async Task StoreInterestRateAsync(MonthlyPerformance monthlyPerformance)
    {
        IMongoCollection<MonthlyPerformance> performanceCollection = GetDatabaseCollection();
        await performanceCollection.InsertOneAsync(monthlyPerformance);
    }

    public async Task DeleteAllPerformanceAsync()
    {
        IMongoCollection<MonthlyPerformance> performanceCollection = GetDatabaseCollection();
        await performanceCollection.DeleteManyAsync(EmptyFilter);
    }
}