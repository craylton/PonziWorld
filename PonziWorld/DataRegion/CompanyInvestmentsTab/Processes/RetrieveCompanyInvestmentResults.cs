using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.DataRegion.CompanyInvestmentsTab.Processes;

internal class RetrieveCompanyInvestmentResults
    : SagaProcess<
        RetrieveCompanyInvestmentResultsCommand,
        RetrieveCompanyInvestmentResultsCommandPayload,
        CompanyInvestmentResultsRetrieveEvent,
        CompanyInvestmentResultsRetrievedEventPayload>
{
    public static RetrieveCompanyInvestmentResults Process => new();
    private RetrieveCompanyInvestmentResults() { }
}

internal class RetrieveCompanyInvestmentResultsCommand
    : PubSubEvent<RetrieveCompanyInvestmentResultsCommandPayload>
{ }

internal record RetrieveCompanyInvestmentResultsCommandPayload(
    Company.Company Company);

internal class CompanyInvestmentResultsRetrieveEvent
    : PubSubEvent<CompanyInvestmentResultsRetrievedEventPayload>
{ }

internal record CompanyInvestmentResultsRetrievedEventPayload(
    double ProfitFromInvestments);
