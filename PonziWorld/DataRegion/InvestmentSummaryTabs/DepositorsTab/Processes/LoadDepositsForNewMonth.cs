using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.DepositorsTab.Processes;

internal class LoadDepositsForNewMonth
    : SagaProcess<
        DepositsForNewMonthLoadedEvent,
        DepositsForNewMonthLoadedEventPayload,
        LoadDepositsForNewMonthCommand,
        LoadDepositsForNewMonthCommandPayload>
{
    public static LoadDepositsForNewMonth Process => new();
    private LoadDepositsForNewMonth() { }
}

internal class DepositsForNewMonthLoadedEvent
    : PubSubEvent<DepositsForNewMonthLoadedEventPayload>
{ }

internal record DepositsForNewMonthLoadedEventPayload(
    IEnumerable<DetailedInvestment> LastMonthDeposits);

internal class LoadDepositsForNewMonthCommand
    : PubSubEvent<LoadDepositsForNewMonthCommandPayload>
{ }

internal record LoadDepositsForNewMonthCommandPayload(
    NewInvestmentsSummary NewInvestmentsSummary);
