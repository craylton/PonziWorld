using PonziWorld.Company.Processes;
using PonziWorld.Core;
using Prism.Events;
using System.Threading.Tasks;

namespace PonziWorld.Company;

internal class CompanyViewModel : BindableSubscriberBase
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
        : base(eventAggregator)
    {
        this.repository = repository;

        SubscribeToProcess(StartNewCompany.Process, CreateCompanyAsync);
        SubscribeToProcess(LoadCompany.Process, LoadCompanyAsync);
        SubscribeToProcess(UpdateCompanyFunds.Process, UpdateFundsAsync);
        SubscribeToProcess(ApplyCompanyInvestmentResults.Process, OnInvestmentProfitsReceived);
        SubscribeToProcess(ApplyClaimedInterestRateToCompany.Process, ApplyClaimedInterestRate);
    }

    private async Task<NewCompanyStartedEventPayload> CreateCompanyAsync(StartNewCompanyCommandPayload payload)
    {
        Company newCompany = new(payload.NewCompanyName, 0, 0, 0, 10, 1, 5);
        await repository.CreateNewCompanyAsync(newCompany);
        Company = newCompany;
        return new(newCompany);
    }

    private async Task<CompanyLoadedEventPayload> LoadCompanyAsync(LoadCompanyCommandPayload _)
    {
        Company = await repository.GetCompanyAsync();
        return new(Company);
    }

    private async Task<CompanyFundsUpdatedEventPayload> UpdateFundsAsync(UpdateCompanyFundsCommandPayload payload)
    {
        double delta = payload.NewInvestmentsSummary.GetTotalInvestment();
        await repository.MoveToNextMonthAsync(delta);
        Company = await repository.GetCompanyAsync();
        return new(Company);
    }

    private async Task<CompanyInvestmentResultsAppliedEventPayload> OnInvestmentProfitsReceived(
        ApplyCompanyInvestmentResultsCommandPayload payload)
    {
        await repository.AddProfitAsync(payload.ProfitFromInvestments);
        return new();
    }

    private async Task<ClaimedInterestRateAppliedToCompanyEventPayload> ApplyClaimedInterestRate(
        ApplyClaimedInterestRateToCompanyCommandPayload payload)
    {
        await repository.ClaimInterest((payload.ClaimedInterestRate / 100d) + 1);
        Company = await repository.GetCompanyAsync();
        return new(Company);
    }
}
