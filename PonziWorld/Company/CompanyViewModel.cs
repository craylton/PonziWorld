using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace PonziWorld.Company;

internal class CompanyViewModel : BindableBase
{
    private readonly ICompanyRepository repository;
    private Company _company = Company.Default;

    public Company Company
    {
        get => _company;
        set => SetProperty(ref _company, value);
    }

    public CompanyViewModel(
        ICompanyRepository repository,
        IEventAggregator eventAggregator)
    {
        this.repository = repository;

        eventAggregator.GetEvent<LoadGameRequestedEvent>()
            .Subscribe(() => LoadCompanyAsync().Await());

        eventAggregator.GetEvent<NewGameInitiatedEvent>()
            .Subscribe(companyName => CreateCompanyAsync(companyName).Await());

        eventAggregator.GetEvent<NextMonthRequestedEvent>()
            .Subscribe(investmentsSummary => UpdateFundsAsync(investmentsSummary).Await());
    }

    private async Task LoadCompanyAsync() =>
        Company = await repository.GetCompanyAsync();

    private async Task CreateCompanyAsync(string companyName)
    {
        var newCompany = new Company(companyName, 0, 0, 0, 10, 1, 5);
        await repository.CreateNewCompanyAsync(newCompany);
        Company = newCompany;
    }

    private async Task UpdateFundsAsync(NewInvestmentsSummary investmentsSummary)
    {
        int companyFunds = Company.ActualFunds;

        foreach (var newInvestor in investmentsSummary.NewInvestors)
        {
            companyFunds += newInvestor.Investment;
        }
        foreach (var reinvestment in investmentsSummary.Reinvestments)
        {
            companyFunds += reinvestment.Amount;
        }
        foreach (var withdrawal in investmentsSummary.Withdrawals)
        {
            companyFunds += withdrawal.Amount;
        }

        await repository.UpdateFundsAsync(companyFunds);
        Company = await repository.GetCompanyAsync();
    }
}
