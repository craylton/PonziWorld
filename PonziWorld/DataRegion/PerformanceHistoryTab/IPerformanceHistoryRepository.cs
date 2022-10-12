using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.PerformanceHistoryTab;

internal interface IPerformanceHistoryRepository
{
    Task StoreInterestRateAsync(MonthlyPerformance monthlyPerformance);
    Task<IEnumerable<MonthlyPerformance>> GetInterestRateHistoryAsync();
    Task DeleteAllPerformanceAsync();
}