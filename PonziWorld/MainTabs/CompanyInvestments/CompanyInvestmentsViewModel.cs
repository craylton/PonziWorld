using PonziWorld.Core;
using PonziWorld.MainTabs.CompanyInvestments.Processes;
using Prism.Events;

namespace PonziWorld.MainTabs.CompanyInvestments;

internal class CompanyInvestmentsViewModel : BindableSubscriberBase
{
    public CompanyInvestmentsViewModel(
        IEventAggregator eventAggregator)
        : base(eventAggregator) =>
        SubscribeToProcess(RetrieveCompanyInvestmentResults.Process, CalculateMonthlyProfit);

    private CompanyInvestmentResultsRetrievedEventPayload CalculateMonthlyProfit(
        RetrieveCompanyInvestmentResultsCommandPayload payload) =>
        new(payload.Company.ActualFunds * 0.005);
}
