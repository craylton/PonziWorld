using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.MainTabs.PerformanceHistory.Processes;

internal class StoreClaimedInterestRate
    : SagaProcess<
        StoreClaimedInterestRateCommand,
        StoreClaimedInterestRateCommandPayload,
        ClaimedInterestRateStoredEvent,
        ClaimedInterestRateStoredEventPayload>
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
