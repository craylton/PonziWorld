using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Events;

internal class LoadDepositsProcess
    : SagaProcess<
        DepositsLoadedEvent,
        DepositsLoadedEventPayload,
        LoadDepositsCommand,
        LoadDepositsCommandPayload>
{ }

internal class DepositsLoadedEvent
    : PubSubEvent<DepositsLoadedEventPayload>
{ }

internal record DepositsLoadedEventPayload;

internal class LoadDepositsCommand
    : PubSubEvent<LoadDepositsCommandPayload>
{ }

internal record LoadDepositsCommandPayload(
    IEnumerable<Investment> Investments);
