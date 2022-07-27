using Prism.Events;

namespace PonziWorld.Events;

internal class LoadInvestors
    : SagaProcess<
        InvestorsLoadedEvent,
        InvestorsLoadedEventPayload,
        LoadInvestorsCommand,
        LoadInvestorsCommandPayload>
{
    public static LoadInvestors Process => new();
    private LoadInvestors() { }
}

internal class InvestorsLoadedEvent
    : PubSubEvent<InvestorsLoadedEventPayload>
{ }

internal record InvestorsLoadedEventPayload;

internal class LoadInvestorsCommand
    : PubSubEvent<LoadInvestorsCommandPayload>
{ }

internal record LoadInvestorsCommandPayload;
