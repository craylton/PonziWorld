using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.MainTabs.MonthlyInvestments.Processes;

internal class LoadDeposits
    : SagaProcess<
        LoadDepositsCommand,
        LoadDepositsCommandPayload,
        DepositsLoadedEvent,
        DepositsLoadedEventPayload>
{
    public static LoadDeposits Process => new();
    private LoadDeposits() { }
}

internal class LoadDepositsCommand
    : PubSubEvent<LoadDepositsCommandPayload>
{ }

internal record LoadDepositsCommandPayload(
    IEnumerable<Investment> LastMonthInvestments);

internal class DepositsLoadedEvent
    : PubSubEvent<DepositsLoadedEventPayload>
{ }

internal record DepositsLoadedEventPayload(
    IEnumerable<DetailedInvestment> LastMonthDeposits);
