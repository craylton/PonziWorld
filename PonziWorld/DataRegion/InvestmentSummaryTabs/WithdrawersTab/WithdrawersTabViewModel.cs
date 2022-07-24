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
    private readonly IInvestorsRepository investorRepository;
    private ObservableCollection<DetailedInvestment> _withdrawals = new();

    public ObservableCollection<DetailedInvestment> Withdrawals
    {
        get => _withdrawals;
        set => SetProperty(ref _withdrawals, value);
    }

    public WithdrawersTabViewModel(
        IInvestorsRepository investorRepository,
        IEventAggregator eventAggregator)
    {
        this.investorRepository = investorRepository;

        eventAggregator.GetEvent<NextMonthRequestedEvent>()
            .Subscribe(investmentsSummary => CompileWithdrawalList(investmentsSummary).Await());
    }

    private async Task CompileWithdrawalList(NewInvestmentsSummary investmentsSummary)
    {
        List<DetailedInvestment> withdrawals = await GetAllNewWithdrawals(investmentsSummary);
        Withdrawals.Clear();
        Withdrawals.AddRange(withdrawals.OrderByDescending(withdrawal => withdrawal.InvestmentSize));
    }

    private async Task<List<DetailedInvestment>> GetAllNewWithdrawals(NewInvestmentsSummary investmentsSummary)
    {
        List<DetailedInvestment> withdrawals = new();

        foreach (Investment withdrawal in investmentsSummary.Withdrawals)
        {
            Investor withdrawer = await investorRepository.GetInvestorByIdAsync(withdrawal.InvestorId);
            withdrawals.Add(new DetailedInvestment(
                withdrawer.Name,
                Math.Abs(withdrawal.Amount),
                withdrawer.Investment + withdrawal.Amount));
        }

        return withdrawals;
    }
}
