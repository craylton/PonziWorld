using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Events;

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

internal record DepositsLoadedEventPayload;

internal class LoadDepositsCommand
    : PubSubEvent<LoadDepositsCommandPayload>
{ }

internal record LoadDepositsCommandPayload(
    IEnumerable<Investment> Investments);
