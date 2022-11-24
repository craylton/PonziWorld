using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;

namespace PonziWorld.DataRegion.TimeAdvancement.Processes;

internal class ApplyNewMonthInvestments
    : SagaProcess<
        ApplyNewMonthInvestmentsCommand,
        ApplyNewMonthInvestmentsCommandPayload,
        NewMonthInvestmentsAppliedEvent,
        NewMonthInvestmentsAppliedEventPayload>
{
    public static ApplyNewMonthInvestments Process => new();
    private ApplyNewMonthInvestments() { }
}

internal class ApplyNewMonthInvestmentsCommand
    : PubSubEvent<ApplyNewMonthInvestmentsCommandPayload>
{ }

internal record ApplyNewMonthInvestmentsCommandPayload(
    NewInvestmentsSummary NewInvestmentsSummary);

internal class NewMonthInvestmentsAppliedEvent
    : PubSubEvent<NewMonthInvestmentsAppliedEventPayload>
{ }

internal record NewMonthInvestmentsAppliedEventPayload();
