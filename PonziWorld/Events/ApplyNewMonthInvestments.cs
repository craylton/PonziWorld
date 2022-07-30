using PonziWorld.Investments;
using Prism.Events;

namespace PonziWorld.Events;

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
