using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.DataRegion.PerformanceHistoryTab.Processes;

internal class StoreClaimedInterestRate
    : SagaProcess<
        ClaimedInterestRateStoredEvent,
        ClaimedInterestRateStoredEventPayload,
        StoreClaimedInterestRateCommand,
        StoreClaimedInterestRateCommandPayload>
{
    public static StoreClaimedInterestRate Process => new();
    private StoreClaimedInterestRate() { }
}

internal class StoreClaimedInterestRateCommand
    : PubSubEvent<StoreClaimedInterestRateCommandPayload>
{ }

internal record StoreClaimedInterestRateCommandPayload(
    int Month,
    double InterestRate);

internal class ClaimedInterestRateStoredEvent
    : PubSubEvent<ClaimedInterestRateStoredEventPayload>
{ }

internal record ClaimedInterestRateStoredEventPayload;
