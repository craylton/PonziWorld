using PonziWorld.Events;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Investments.Investors.Processes;

internal class RetrieveInvestors
    : SagaProcess<
        RetrieveInvestorsCommand,
        RetrieveInvestorsCommandPayload,
        InvestorsRetrievedEvent,
        InvestorsRetrievedEventPayload>
{
    public static RetrieveInvestors Process => new();
    private RetrieveInvestors() { }
}

internal class RetrieveInvestorsCommand
    : PubSubEvent<RetrieveInvestorsCommandPayload>
{ }

internal record RetrieveInvestorsCommandPayload;

internal class InvestorsRetrievedEvent
    : PubSubEvent<InvestorsRetrievedEventPayload>
{ }

internal record InvestorsRetrievedEventPayload(
    IEnumerable<Investor> Investors);
