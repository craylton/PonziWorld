using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Events;

internal class WithdrawalsLoadedEvent
    : PubSubEvent
{ }

internal class LoadWithdrawalsCommand
    : PubSubEvent<LoadWithdrawalsCommandPayload>
{ }

internal record LoadWithdrawalsCommandPayload(
    IEnumerable<Investment> LastMonthInvestments);
