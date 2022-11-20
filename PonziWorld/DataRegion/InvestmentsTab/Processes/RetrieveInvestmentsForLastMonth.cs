using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.DataRegion.InvestmentsTab.Processes;

internal class RetrieveInvestmentsForLastMonth
    : SagaProcess<
        InvestmentsForLastMonthRetrievedEvent,
        InvestmentsForLastMonthRetrievedEventPayload,
        RetrieveInvestmentsForLastMonthCommand,
        RetrieveInvestmentsForLastMonthCommandPayload>
{
    public static RetrieveInvestmentsForLastMonth Process => new();
    private RetrieveInvestmentsForLastMonth() { }
}

internal class InvestmentsForLastMonthRetrievedEvent
    : PubSubEvent<InvestmentsForLastMonthRetrievedEventPayload>
{ }

internal record InvestmentsForLastMonthRetrievedEventPayload(
    IEnumerable<Investment> LastMonthInvestments);

internal class RetrieveInvestmentsForLastMonthCommand
    : PubSubEvent<RetrieveInvestmentsForLastMonthCommandPayload>
{ }

internal record RetrieveInvestmentsForLastMonthCommandPayload(
    int CurrentMonth);
