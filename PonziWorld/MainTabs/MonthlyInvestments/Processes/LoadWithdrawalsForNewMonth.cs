using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.MainTabs.MonthlyInvestments.Processes;

internal class LoadWithdrawalsForNewMonth
    : SagaProcess<
        LoadWithdrawalsForNewMonthCommand,
        LoadWithdrawalsForNewMonthCommandPayload,
        WithdrawalsForNewMonthLoadedEvent,
        WithdrawalsForNewMonthLoadedEventPayload>
{
    public static LoadWithdrawalsForNewMonth Process => new();
    private LoadWithdrawalsForNewMonth() { }
}

internal class LoadWithdrawalsForNewMonthCommand
    : PubSubEvent<LoadWithdrawalsForNewMonthCommandPayload>
{ }

internal record LoadWithdrawalsForNewMonthCommandPayload(
    NewInvestmentsSummary NewInvestmentsSummary);

internal class WithdrawalsForNewMonthLoadedEvent
    : PubSubEvent<WithdrawalsForNewMonthLoadedEventPayload>
{ }

internal record WithdrawalsForNewMonthLoadedEventPayload(
    IEnumerable<DetailedInvestment> LastMonthWithdrawals);
