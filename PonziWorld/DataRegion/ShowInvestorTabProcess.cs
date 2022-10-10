using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Investments.Investors;

internal class DisplayInvestorTab
    : SagaProcess<
        InvestorDisplayedEvent,
        InvestorDisplayedEventPayload,
        DisplayInvestorTabCommand,
        DisplayInvestorTabCommandPayload>
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
