using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Investments.Investors.Processes;

internal class RetrieveSelectedInvestor
    : SagaProcess<
        RetrieveSelectedInvestorEvent,
        RetrieveSelectedInvestorEventPayload,
        RetrieveSelectedInvestorCommand,
        RetrieveSelectedInvestorCommandPayload>
{
    public static RetrieveSelectedInvestor Process => new();
    private RetrieveSelectedInvestor() { }
}

internal class RetrieveSelectedInvestorCommand
    : PubSubEvent<RetrieveSelectedInvestorCommandPayload>
{ }

internal record RetrieveSelectedInvestorCommandPayload;

internal class RetrieveSelectedInvestorEvent
    : PubSubEvent<RetrieveSelectedInvestorEventPayload>
{ }

internal record RetrieveSelectedInvestorEventPayload(
    Investor? Investor);
