using PonziWorld.Core;
using PonziWorld.MainTabs.PerformanceHistory.Processes;
using Prism.Events;
using System.Threading.Tasks;

namespace PonziWorld.MainTabs.PerformanceHistory;

internal class PerformanceHistoryViewModel : BindableSubscriberBase
{
    private readonly IPerformanceHistoryRepository performanceHistoryRepository;

    public PerformanceHistoryViewModel(
        IPerformanceHistoryRepository performanceHistoryRepository,
        IEventAggregator eventAggregator)
        : base(eventAggregator)
    {
        this.performanceHistoryRepository = performanceHistoryRepository;

        SubscribeToProcess(RetrieveInterestRateHistory.Process, GetHistoricalPerformance);
        SubscribeToProcess(StoreClaimedInterestRate.Process, StoreInterestRateAsync);
        SubscribeToProcess(ClearPerformance.Process, DeletePerformanceHistoryAsync);
    }

    private async Task<InterestRateHistoryRetrievedPayload> GetHistoricalPerformance(
        RetrieveInterestRateHistoryCommandPayload _)
    {
        var performance = await performanceHistoryRepository.GetInterestRateHistoryAsync();
        return new(performance);
    }

    private async Task<ClaimedInterestRateStoredEventPayload> StoreInterestRateAsync(
        StoreClaimedInterestRateCommandPayload payload)
    {
        var performance = new MonthlyPerformance(payload.Month, payload.InterestRate);
        await performanceHistoryRepository.StoreInterestRateAsync(performance);
        return new();
    }

    private async Task<PerformanceClearedEventPayload> DeletePerformanceHistoryAsync(
        ClearPerformanceCommandPayload _)
    {
        await performanceHistoryRepository.DeleteAllPerformanceAsync();
        return new();
    }
}
