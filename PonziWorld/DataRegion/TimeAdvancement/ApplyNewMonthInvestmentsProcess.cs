using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;

namespace PonziWorld.DataRegion.TimeAdvancement;

internal class ApplyNewMonthInvestments
    : SagaProcess<
        NewMonthInvestmentsAppliedEvent,
        NewMonthInvestmentsAppliedEventPayload,
        ApplyNewMonthInvestmentsCommand,
        ApplyNewMonthInvestmentsCommandPayload>
{
    public static ApplyNewMonthInvestments Process => new();
    private ApplyNewMonthInvestments() { }
}

internal class NewMonthInvestmentsAppliedEvent
    : PubSubEvent<NewMonthInvestmentsAppliedEventPayload>
{ }

internal record NewMonthInvestmentsAppliedEventPayload();

internal class ApplyNewMonthInvestmentsCommand
    : PubSubEvent<ApplyNewMonthInvestmentsCommandPayload>
{ }

internal record ApplyNewMonthInvestmentsCommandPayload(
    NewInvestmentsSummary NewInvestmentsSummary);
