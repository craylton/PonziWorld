using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.MainTabs.MonthlyInvestments.Processes;

internal class RetrieveInvestmentsForLastMonth
    : SagaProcess<
        RetrieveInvestmentsForLastMonthCommand,
        RetrieveInvestmentsForLastMonthCommandPayload,
        InvestmentsForLastMonthRetrievedEvent,
        InvestmentsForLastMonthRetrievedEventPayload>
{
    public static RetrieveInvestmentsForLastMonth Process => new();
    private RetrieveInvestmentsForLastMonth() { }
}

internal class RetrieveInvestmentsForLastMonthCommand
    : PubSubEvent<RetrieveInvestmentsForLastMonthCommandPayload>
{ }

internal record RetrieveInvestmentsForLastMonthCommandPayload(
    int CurrentMonth);

internal class InvestmentsForLastMonthRetrievedEvent
    : PubSubEvent<InvestmentsForLastMonthRetrievedEventPayload>
{ }

internal record InvestmentsForLastMonthRetrievedEventPayload(
    IEnumerable<Investment> LastMonthInvestments);
