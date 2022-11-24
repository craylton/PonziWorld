using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.WithdrawersTab.Processes;

internal class LoadWithdrawals
    : SagaProcess<
        LoadWithdrawalsCommand,
        LoadWithdrawalsCommandPayload,
        WithdrawalsLoadedEvent,
        WithdrawalsLoadedEventPayload>
{
    public static LoadWithdrawals Process => new();
    private LoadWithdrawals() { }
}

internal class LoadWithdrawalsCommand
    : PubSubEvent<LoadWithdrawalsCommandPayload>
{ }

internal record LoadWithdrawalsCommandPayload(
    IEnumerable<Investment> LastMonthInvestments);

internal class WithdrawalsLoadedEvent
    : PubSubEvent<WithdrawalsLoadedEventPayload>
{ }

internal record WithdrawalsLoadedEventPayload(
    IEnumerable<DetailedInvestment> LastMonthWithdrawals);
