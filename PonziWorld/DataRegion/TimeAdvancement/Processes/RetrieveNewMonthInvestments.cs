using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.DataRegion.TimeAdvancement.Processes;

internal class RetrieveNewMonthInvestments
    : SagaProcess<
        RetrieveNewMonthInvestmentsCommand,
        RetrieveNewMonthInvestmentsCommandPayload,
        NewMonthInvestmentsRetrievedEvent,
        NewMonthInvestmentsRetrievedEventPayload>
{
    public static RetrieveNewMonthInvestments Process => new();
    private RetrieveNewMonthInvestments() { }
}

internal class RetrieveNewMonthInvestmentsCommand
    : PubSubEvent<RetrieveNewMonthInvestmentsCommandPayload>
{ }

internal record RetrieveNewMonthInvestmentsCommandPayload(
    Company.Company Company,
    IEnumerable<Investor> CurrentInvestors);

internal class NewMonthInvestmentsRetrievedEvent
    : PubSubEvent<NewMonthInvestmentsRetrievedEventPayload>
{ }

internal record NewMonthInvestmentsRetrievedEventPayload(
    NewInvestmentsSummary NewInvestmentsSummary);
