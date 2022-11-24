using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.DataRegion.CompanyInvestmentsTab.Processes;

internal class DetermineCompanyInvestmentResults
    : SagaProcess<
        DetermineCompanyInvestmentResultsCommand,
        DetermineCompanyInvestmentResultsCommandPayload,
        CompanyInvestmentResultsDeterminedEvent,
        CompanyInvestmentResultsDeterminedEventPayload>
{
    public static DetermineCompanyInvestmentResults Process => new();
    private DetermineCompanyInvestmentResults() { }
}

internal class DetermineCompanyInvestmentResultsCommand
    : PubSubEvent<DetermineCompanyInvestmentResultsCommandPayload>
{ }

internal record DetermineCompanyInvestmentResultsCommandPayload(
    Company.Company Company);

internal class CompanyInvestmentResultsDeterminedEvent
    : PubSubEvent<CompanyInvestmentResultsDeterminedEventPayload>
{ }

internal record CompanyInvestmentResultsDeterminedEventPayload(
    double ProfitFromInvestments);
