using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Events;

internal class LoadWithdrawalsProcess
    : SagaProcess<
        WithdrawalsLoadedEvent,
        WithdrawalsLoadedEventPayload,
        LoadWithdrawalsCommand,
        LoadWithdrawalsCommandPayload>
{ }

internal class WithdrawalsLoadedEvent
    : PubSubEvent<WithdrawalsLoadedEventPayload>
{ }

internal record WithdrawalsLoadedEventPayload;

internal class LoadWithdrawalsCommand
    : PubSubEvent<LoadWithdrawalsCommandPayload>
{ }

internal record LoadWithdrawalsCommandPayload(
    IEnumerable<Investment> LastMonthInvestments);
