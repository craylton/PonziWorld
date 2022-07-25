using PonziWorld.Company;
using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.WithdrawersTab;

internal class WithdrawersTabViewModel : BindableBase
{
    private readonly IInvestorsRepository investorsRepository;
    private readonly ICompanyRepository companyRepository;
    private readonly IInvestmentsRepository investmentsRepository;
    private ObservableCollection<DetailedInvestment> _withdrawals = new();

    public ObservableCollection<DetailedInvestment> Withdrawals
    {
        get => _withdrawals;
        set => SetProperty(ref _withdrawals, value);
    }

    public WithdrawersTabViewModel(
        IInvestorsRepository investorsRepository,
        ICompanyRepository companyRepository,
        IInvestmentsRepository investmentsRepository,
        IEventAggregator eventAggregator)
    {
        this.investorsRepository = investorsRepository;
        this.companyRepository = companyRepository;
        this.investmentsRepository = investmentsRepository;

        eventAggregator.GetEvent<LoadGameRequestedEvent>()
            .Subscribe(() => LoadLastMonthWithdrawals().Await());

        eventAggregator.GetEvent<NextMonthRequestedEvent>()
            .Subscribe(investmentsSummary => CompileWithdrawalList(investmentsSummary).Await());
    }

    private async Task LoadLastMonthWithdrawals()
    {
        Company.Company company = await companyRepository.GetCompanyAsync();

        IEnumerable<Investment> lastMonthWithdrawals = (await investmentsRepository
            .GetInvestmentsByMonthAsync(company.Month - 1))
            .Where(investment => investment.Amount < 0);

        List<DetailedInvestment> investments = await GetDetailedWithdrawals(lastMonthWithdrawals);
        SetWithdrawalsList(investments);
    }

    private async Task CompileWithdrawalList(NewInvestmentsSummary investmentsSummary)
    {
        List<DetailedInvestment> withdrawals = await GetAllNewWithdrawals(investmentsSummary);
        SetWithdrawalsList(withdrawals);
    }

    private void SetWithdrawalsList(List<DetailedInvestment> withdrawals)
    {
        Withdrawals.Clear();
        Withdrawals.AddRange(withdrawals.OrderByDescending(withdrawal => withdrawal.InvestmentSize));
    }

    private async Task<List<DetailedInvestment>> GetAllNewWithdrawals(NewInvestmentsSummary investmentsSummary) =>
        await GetDetailedWithdrawals(investmentsSummary.Withdrawals);

    private async Task<List<DetailedInvestment>> GetDetailedWithdrawals(IEnumerable<Investment> withdrawals)
    {
        List<DetailedInvestment> detailedInvestments = new();

        foreach (Investment withdrawal in withdrawals)
        {
            Investor investor = await investorsRepository.GetInvestorByIdAsync(withdrawal.InvestorId);
            detailedInvestments.Add(new DetailedInvestment(
                investor.Name,
                Math.Abs(withdrawal.Amount),
                investor.Investment - withdrawal.Amount));
        }

        return detailedInvestments;
    }
}
