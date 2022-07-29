using Prism.Events;

namespace PonziWorld.Events;

internal class ClearInvestors
    : SagaProcess<
        InvestorsClearedEvent,
        InvestorsClearedEventPayload,
        ClearInvestorsCommand,
        ClearInvestorsCommandPayload>
{
    public static ClearInvestors Process => new();
    private ClearInvestors() { }
}

internal class ClearInvestorsCommand
    : PubSubEvent<ClearInvestorsCommandPayload>
{ }

internal record ClearInvestorsCommandPayload;

internal class InvestorsClearedEvent
    : PubSubEvent<InvestorsClearedEventPayload>
{ }

internal record InvestorsClearedEventPayload;
