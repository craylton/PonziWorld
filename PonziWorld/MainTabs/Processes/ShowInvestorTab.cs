using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.MainTabs.Processes;

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
    Investments.Investors.Investor? Investor);

internal class InvestorTabDisplayedEvent
    : PubSubEvent<InvestorTabDisplayedEventPayload>
{ }

internal record InvestorTabDisplayedEventPayload;
