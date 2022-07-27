using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Events;

internal class LoadWithdrawals
    : SagaProcess<
        WithdrawalsLoadedEvent,
        WithdrawalsLoadedEventPayload,
        LoadWithdrawalsCommand,
        LoadWithdrawalsCommandPayload>
{
    public static LoadWithdrawals Process => new();
    private LoadWithdrawals() { }
}

internal class WithdrawalsLoadedEvent
    : PubSubEvent<WithdrawalsLoadedEventPayload>
{ }

internal record WithdrawalsLoadedEventPayload;

internal class LoadWithdrawalsCommand
    : PubSubEvent<LoadWithdrawalsCommandPayload>
{ }

internal record LoadWithdrawalsCommandPayload(
    IEnumerable<Investment> LastMonthInvestments);
