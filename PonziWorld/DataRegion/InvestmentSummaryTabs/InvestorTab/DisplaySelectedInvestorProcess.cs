﻿using PonziWorld.DataRegion.PerformanceHistoryTab;
using PonziWorld.Events;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Investments.Investors;

internal class DisplayInvestor
    : SagaProcess<
        InvestorDisplayedEvent,
        InvestorDisplayedEventPayload,
        DisplayInvestorCommand,
        DisplayInvestorCommandPayload>
{
    public static DisplayInvestor Process => new();
    private DisplayInvestor() { }
}

internal class DisplayInvestorCommand
    : PubSubEvent<DisplayInvestorCommandPayload>
{ }

internal record DisplayInvestorCommandPayload(
    Investor Investor,
    IEnumerable<MonthlyPerformance> interestRateHistory);

internal class InvestorDisplayedEvent
    : PubSubEvent<InvestorDisplayedEventPayload>
{ }

internal record InvestorDisplayedEventPayload;
