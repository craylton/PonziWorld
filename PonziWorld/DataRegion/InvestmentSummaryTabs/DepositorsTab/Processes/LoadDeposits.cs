using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.DepositorsTab.Processes;

internal class LoadDeposits
    : SagaProcess<
        DepositsLoadedEvent,
        DepositsLoadedEventPayload,
        LoadDepositsCommand,
        LoadDepositsCommandPayload>
{
    public static LoadDeposits Process => new();
    private LoadDeposits() { }
}

internal class DepositsLoadedEvent
    : PubSubEvent<DepositsLoadedEventPayload>
{ }

internal record DepositsLoadedEventPayload(
    IEnumerable<DetailedInvestment> LastMonthDeposits);

internal class LoadDepositsCommand
    : PubSubEvent<LoadDepositsCommandPayload>
{ }

internal record LoadDepositsCommandPayload(
    IEnumerable<Investment> LastMonthInvestments);
