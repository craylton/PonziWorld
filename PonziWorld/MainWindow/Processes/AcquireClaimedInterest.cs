using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.MainWindow.Processes;

internal class AcquireClaimedInterest
    : SagaProcess<
        AcquireClaimedInterestCommand,
        AcquireClaimedInterestCommandPayload,
        ClaimedInterestAcquiredEvent,
        ClaimedInterestAcquiredEventPayload>
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
    double ClaimedInterestRate);
