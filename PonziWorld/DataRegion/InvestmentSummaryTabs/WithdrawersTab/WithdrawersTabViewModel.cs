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
            .Subscribe(() => LoadLastMonthWithdrawalsAsync().Await());

        eventAggregator.GetEvent<NextMonthRequestedEvent>()
            .Subscribe(investmentsSummary => CompileWithdrawalListAsync(investmentsSummary).Await());
    }

    private async Task LoadLastMonthWithdrawalsAsync()
    {
        Company.Company company = await companyRepository.GetCompanyAsync();

        IEnumerable<Investment> lastMonthWithdrawals = (await investmentsRepository
            .GetInvestmentsByMonthAsync(company.Month - 1))
            .Where(investment => investment.Amount < 0);

        IEnumerable<DetailedInvestment> investments = await GetDetailedWithdrawalsAsync(lastMonthWithdrawals);
        SetWithdrawalsList(investments);
    }

    private async Task CompileWithdrawalListAsync(NewInvestmentsSummary investmentsSummary)
    {
        IEnumerable<DetailedInvestment> withdrawals = await GetAllNewWithdrawalsAsync(investmentsSummary);
        SetWithdrawalsList(withdrawals);
    }

    private void SetWithdrawalsList(IEnumerable<DetailedInvestment> withdrawals)
    {
        Withdrawals.Clear();
        Withdrawals.AddRange(withdrawals.OrderByDescending(withdrawal => withdrawal.InvestmentSize));
    }

    private async Task<IEnumerable<DetailedInvestment>> GetAllNewWithdrawalsAsync(NewInvestmentsSummary investmentsSummary) =>
        await GetDetailedWithdrawalsAsync(investmentsSummary.Withdrawals);

    private async Task<IEnumerable<DetailedInvestment>> GetDetailedWithdrawalsAsync(IEnumerable<Investment> withdrawals)
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
