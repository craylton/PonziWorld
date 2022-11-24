using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.DepositorsTab.Processes;

internal class LoadDepositsForNewMonth
    : SagaProcess<
        LoadDepositsForNewMonthCommand,
        LoadDepositsForNewMonthCommandPayload,
        DepositsForNewMonthLoadedEvent,
        DepositsForNewMonthLoadedEventPayload>
{
    public static LoadDepositsForNewMonth Process => new();
    private LoadDepositsForNewMonth() { }
}

internal class LoadDepositsForNewMonthCommand
    : PubSubEvent<LoadDepositsForNewMonthCommandPayload>
{ }

internal record LoadDepositsForNewMonthCommandPayload(
    NewInvestmentsSummary NewInvestmentsSummary);

internal class DepositsForNewMonthLoadedEvent
    : PubSubEvent<DepositsForNewMonthLoadedEventPayload>
{ }

internal record DepositsForNewMonthLoadedEventPayload(
    IEnumerable<DetailedInvestment> LastMonthDeposits);
