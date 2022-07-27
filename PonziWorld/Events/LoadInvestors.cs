using Prism.Events;

namespace PonziWorld.Events;

internal class LoadInvestorsProcess
    : SagaProcess<
        InvestorsLoadedEvent,
        InvestorsLoadedEventPayload,
        LoadInvestorsCommand,
        LoadInvestorsCommandPayload>
{ }

internal class InvestorsLoadedEvent
    : PubSubEvent<InvestorsLoadedEventPayload>
{ }

internal record InvestorsLoadedEventPayload;

internal class LoadInvestorsCommand
    : PubSubEvent<LoadInvestorsCommandPayload>
{ }

internal record LoadInvestorsCommandPayload;
