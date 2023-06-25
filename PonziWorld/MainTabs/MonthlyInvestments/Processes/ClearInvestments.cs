using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.MainTabs.MonthlyInvestments.Processes;

internal class ClearInvestments
    : SagaProcess<
        ClearInvestmentsCommand,
        ClearInvestmentsCommandPayload,
        InvestmentsClearedEvent,
        InvestmentsClearedEventPayload>
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
