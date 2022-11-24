using PonziWorld.Events;
using PonziWorld.Investments.Investors;
using Prism.Events;

namespace PonziWorld.DataRegion.Processes;

internal class DisplayInvestorTab
    : SagaProcess<
        DisplayInvestorTabCommand,
        DisplayInvestorTabCommandPayload,
        InvestorTabDisplayedEvent,
        InvestorTabDisplayedEventPayload>
{
    public static DisplayInvestorTab Process => new();
    private DisplayInvestorTab() { }
}

internal class DisplayInvestorTabCommand
    : PubSubEvent<DisplayInvestorTabCommandPayload>
{ }

internal record DisplayInvestorTabCommandPayload(
    Investor? Investor);

internal class InvestorTabDisplayedEvent
    : PubSubEvent<InvestorTabDisplayedEventPayload>
{ }

internal record InvestorTabDisplayedEventPayload;
