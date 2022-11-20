using PonziWorld.Events;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Investments.Investors.Processes;

internal class RetrieveInvestors
    : SagaProcess<
        InvestorsRetrievedEvent,
        InvestorsRetrievedEventPayload,
        RetrieveInvestorsCommand,
        RetrieveInvestorsCommandPayload>
{
    public static RetrieveInvestors Process => new();
    private RetrieveInvestors() { }
}

internal class InvestorsRetrievedEvent
    : PubSubEvent<InvestorsRetrievedEventPayload>
{ }

internal record InvestorsRetrievedEventPayload(
    IEnumerable<Investor> Investors);

internal class RetrieveInvestorsCommand
    : PubSubEvent<RetrieveInvestorsCommandPayload>
{ }

internal record RetrieveInvestorsCommandPayload;
