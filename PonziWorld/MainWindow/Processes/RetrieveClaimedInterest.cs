using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.MainWindow.Processes;

internal class RetrieveClaimedInterest
    : SagaProcess<
        RetrieveClaimedInterestCommand,
        RetrieveClaimedInterestCommandPayload,
        ClaimedInterestRetrievedEvent,
        ClaimedInterestRetrievedEventPayload>
{
    public static RetrieveClaimedInterest Process => new();
    private RetrieveClaimedInterest() { }
}

internal class RetrieveClaimedInterestCommand
    : PubSubEvent<RetrieveClaimedInterestCommandPayload>
{ }

internal record RetrieveClaimedInterestCommandPayload;

internal class ClaimedInterestRetrievedEvent
    : PubSubEvent<ClaimedInterestRetrievedEventPayload>
{ }

internal record ClaimedInterestRetrievedEventPayload(
    double ClaimedInterestRate);
