using PonziWorld.Events;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Investments.Investors;

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

internal class LoadInvestorsCommand
    : PubSubEvent<LoadInvestorsCommandPayload>
{ }

internal record LoadInvestorsCommandPayload;

internal class InvestorsLoadedEvent
    : PubSubEvent<InvestorsLoadedEventPayload>
{ }

internal record InvestorsLoadedEventPayload(
    IEnumerable<Investor> ActiveInvestors);
