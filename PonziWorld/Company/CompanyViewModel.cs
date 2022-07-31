using PonziWorld.Core;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System;
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

        SubscribeToProcess(LoadCompany.Process, LoadCompanyAsync);
        SubscribeToProcess(StartNewCompany.Process, CreateCompanyAsync);
        SubscribeToProcess(UpdateCompanyFunds.Process, UpdateFundsAsync);
        SubscribeToProcess(ApplyCompanyInvestmentResults.Process, OnInvestmentProfitsReceived);
    }

    private async Task<CompanyLoadedEventPayload> LoadCompanyAsync(LoadCompanyCommandPayload _)
    {
        Company = await repository.GetCompanyAsync();
        return new(Company);
    }

    private async Task<NewCompanyStartedEventPayload> CreateCompanyAsync(StartNewCompanyCommandPayload payload)
    {
        Company newCompany = new(payload.NewCompanyName, 0, 0, 0, 10, 1, 5);
        await repository.CreateNewCompanyAsync(newCompany);
        Company = newCompany;
        return new(newCompany);
    }

    private async Task<CompanyFundsUpdatedEventPayload> UpdateFundsAsync(UpdateCompanyFundsCommandPayload payload)
    {
        var investmentsSummary = payload.NewInvestmentsSummary;
        int companyFunds = Company.ActualFunds;

        foreach (Investor newInvestor in investmentsSummary.NewInvestors)
        {
            companyFunds += newInvestor.Investment;
        }
        foreach (Investment reinvestment in investmentsSummary.Reinvestments)
        {
            companyFunds += reinvestment.Amount;
        }
        foreach (Investment withdrawal in investmentsSummary.Withdrawals)
        {
            companyFunds += withdrawal.Amount;
        }

        await repository.MoveToNextMonthAsync(companyFunds);
        Company = await repository.GetCompanyAsync();
        return new(Company);
    }

    private CompanyInvestmentResultsAppliedEventPayload OnInvestmentProfitsReceived(
        ApplyCompanyInvestmentResultsCommandPayload payload) =>
        // TODO: add payload.ProfitFromInvestments to ActualProfit, and update stats
        new();
}
