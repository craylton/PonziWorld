using PonziWorld.Events;
using PonziWorld.MainTabs.PerformanceHistory;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.MainTabs.Investor.Processes;

internal class DisplayInvestor
    : SagaProcess<
        DisplayInvestorCommand,
        DisplayInvestorCommandPayload,
        InvestorDisplayedEvent,
        InvestorDisplayedEventPayload>
{
    public static DisplayInvestor Process => new();
    private DisplayInvestor() { }
}

internal class DisplayInvestorCommand
    : PubSubEvent<DisplayInvestorCommandPayload>
{ }

internal record DisplayInvestorCommandPayload(
    Investments.Investors.Investor Investor,
    IEnumerable<MonthlyPerformance> InterestRateHistory);

internal class InvestorDisplayedEvent
    : PubSubEvent<InvestorDisplayedEventPayload>
{ }

internal record InvestorDisplayedEventPayload;
