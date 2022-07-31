using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.MainWindow;

internal class AcquireClaimedInterest
    : SagaProcess<
        ClaimedInterestAcquiredEvent,
        ClaimedInterestAcquiredEventPayload,
        AcquireClaimedInterestCommand,
        AcquireClaimedInterestCommandPayload>
{
    public static AcquireClaimedInterest Process => new();
    private AcquireClaimedInterest() { }
}

internal class AcquireClaimedInterestCommand
    : PubSubEvent<AcquireClaimedInterestCommandPayload>
{ }

internal record AcquireClaimedInterestCommandPayload;

internal class ClaimedInterestAcquiredEvent
    : PubSubEvent<ClaimedInterestAcquiredEventPayload>
{ }

internal record ClaimedInterestAcquiredEventPayload(
    double ClaimedInterest);
