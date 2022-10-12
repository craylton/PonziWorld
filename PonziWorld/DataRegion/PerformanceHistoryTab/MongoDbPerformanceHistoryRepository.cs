using PonziWorld.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.PerformanceHistoryTab;

internal class MongoDbPerformanceHistoryRepository : MongoDbRepositoryBase<MonthlyPerformance>, IPerformanceHistoryRepository
{
    public MongoDbPerformanceHistoryRepository() : base("companyPerformance")
    {
    }

    public async Task StoreInterestRateAsync(MonthlyPerformance monthlyPerformance) =>
        await AddOneAsync(monthlyPerformance);

    public async Task<IEnumerable<MonthlyPerformance>> GetInterestRateHistoryAsync() =>
        await GetAllAsync();

    public async Task DeleteAllPerformanceAsync() =>
        await DeleteAllAsync();
}