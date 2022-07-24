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
    private ObservableCollection<DetailedInvestment> _withdrawers = new();

    public ObservableCollection<DetailedInvestment> Withdrawers
    {
        get => _withdrawers;
        set => SetProperty(ref _withdrawers, value);
    }

    public WithdrawersTabViewModel(
        IInvestorsRepository investorRepository,
        IEventAggregator eventAggregator)
    {
        this.investorRepository = investorRepository;

        eventAggregator.GetEvent<NextMonthRequestedEvent>()
            .Subscribe(investmentsSummary => CompileWithdrawerList(investmentsSummary).Await());
    }

    private async Task CompileWithdrawerList(NewInvestmentsSummary investmentsSummary)
    {
        List<DetailedInvestment> withdrawals = await GetAllNewWithdrawers(investmentsSummary);
        Withdrawers.Clear();
        Withdrawers.AddRange(withdrawals.OrderByDescending(withdrawer => withdrawer.InvestmentSize));
    }

    private async Task<List<DetailedInvestment>> GetAllNewWithdrawers(NewInvestmentsSummary investmentsSummary)
    {
        var withdrawals = new List<DetailedInvestment>();

        foreach (Investment withdrawal in investmentsSummary.Withdrawals)
        {
            var withdrawer = await investorRepository.GetInvestorByIdAsync(withdrawal.InvestorId);
            withdrawals.Add(new DetailedInvestment(
                withdrawer.Name,
                Math.Abs(withdrawal.Amount),
                withdrawer.Investment + withdrawal.Amount));
        }

        return withdrawals;
    }
}
