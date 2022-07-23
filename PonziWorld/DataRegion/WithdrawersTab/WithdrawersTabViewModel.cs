using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.WithdrawersTab;

internal class WithdrawersTabViewModel : BindableBase
{
    private readonly IInvestorsRepository investorRepository;
    private ObservableCollection<Investor> _withdrawers = new();

    public ObservableCollection<Investor> Withdrawers
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
        List<Investor> withdrawers = await GetAllNewWithdrawers(investmentsSummary);
        Withdrawers.Clear();
        Withdrawers.AddRange(withdrawers.OrderBy(withdrawer => withdrawer.Investment));
    }

    private async Task<List<Investor>> GetAllNewWithdrawers(NewInvestmentsSummary investmentsSummary)
    {
        var withdrawers = new List<Investor>();

        foreach (var withdrawal in investmentsSummary.Withdrawals)
        {
            var withdrawer = await investorRepository.GetInvestorByIdAsync(withdrawal.InvestorId);
            withdrawers.Add(withdrawer);
        }

        return withdrawers;
    }
}
