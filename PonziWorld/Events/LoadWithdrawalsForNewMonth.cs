using PonziWorld.Investments;
using Prism.Events;

namespace PonziWorld.Events;

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

internal record WithdrawalsForNewMonthLoadedEventPayload();

internal class LoadWithdrawalsForNewMonthCommand
    : PubSubEvent<LoadWithdrawalsForNewMonthCommandPayload>
{ }

internal record LoadWithdrawalsForNewMonthCommandPayload(
    NewInvestmentsSummary NewInvestmentsSummary);
