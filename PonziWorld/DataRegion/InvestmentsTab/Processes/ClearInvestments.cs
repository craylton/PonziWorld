using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.DataRegion.InvestmentsTab.Processes;

internal class ClearInvestments
    : SagaProcess<
        InvestmentsClearedEvent,
        InvestmentsClearedEventPayload,
        ClearInvestmentsCommand,
        ClearInvestmentsCommandPayload>
{
    public static ClearInvestments Process => new();
    private ClearInvestments() { }
}

internal class ClearInvestmentsCommand
    : PubSubEvent<ClearInvestmentsCommandPayload>
{ }

internal record ClearInvestmentsCommandPayload;

internal class InvestmentsClearedEvent
    : PubSubEvent<InvestmentsClearedEventPayload>
{ }

internal record InvestmentsClearedEventPayload;
