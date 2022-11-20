using PonziWorld.Core;
using PonziWorld.DataRegion.CompanyInvestmentsTab.Processes;
using Prism.Events;

namespace PonziWorld.DataRegion.CompanyInvestmentsTab;

internal class CompanyInvestmentsTabViewModel : BindableSubscriberBase
{
    public CompanyInvestmentsTabViewModel(
        IEventAggregator eventAggregator)
        : base(eventAggregator) =>
        SubscribeToProcess(DetermineCompanyInvestmentResults.Process, CalculateMonthlyProfit);

    private CompanyInvestmentResultsDeterminedEventPayload CalculateMonthlyProfit(
        DetermineCompanyInvestmentResultsCommandPayload payload) =>
        new(payload.Company.ActualFunds * 0.005);
}
