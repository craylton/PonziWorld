using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.WithdrawersTab;

internal class LoadWithdrawalsForNewMonth
    : SagaProcess<
        WithdrawalsForNewMonthLoadedEvent,
        WithdrawalsForNewMonthLoadedEventPayload,
        LoadWithdrawalsForNewMonthCommand,
        LoadWithdrawalsForNewMonthCommandPayload>
{
    public static LoadWithdrawalsForNewMonth Process => new();
    private LoadWithdrawalsForNewMonth() { }
}

internal class WithdrawalsForNewMonthLoadedEvent
    : PubSubEvent<WithdrawalsForNewMonthLoadedEventPayload>
{ }

internal record WithdrawalsForNewMonthLoadedEventPayload(
    IEnumerable<DetailedInvestment> LastMonthWithdrawals);

internal class LoadWithdrawalsForNewMonthCommand
    : PubSubEvent<LoadWithdrawalsForNewMonthCommandPayload>
{ }

internal record LoadWithdrawalsForNewMonthCommandPayload(
    NewInvestmentsSummary NewInvestmentsSummary);
