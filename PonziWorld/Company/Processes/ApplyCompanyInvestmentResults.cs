using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Company.Processes;

internal class ApplyCompanyInvestmentResults
    : SagaProcess<
        ApplyCompanyInvestmentResultsCommand,
        ApplyCompanyInvestmentResultsCommandPayload,
        CompanyInvestmentResultsAppliedEvent,
        CompanyInvestmentResultsAppliedEventPayload>
{
    public static ApplyCompanyInvestmentResults Process => new();
    private ApplyCompanyInvestmentResults() { }
}

internal class ApplyCompanyInvestmentResultsCommand
    : PubSubEvent<ApplyCompanyInvestmentResultsCommandPayload>
{ }

internal record ApplyCompanyInvestmentResultsCommandPayload(
    double ProfitFromInvestments);

internal class CompanyInvestmentResultsAppliedEvent
    : PubSubEvent<CompanyInvestmentResultsAppliedEventPayload>
{ }

internal record CompanyInvestmentResultsAppliedEventPayload;
