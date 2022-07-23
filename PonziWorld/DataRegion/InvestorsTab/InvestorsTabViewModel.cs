using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.InvestorsTab;

internal class InvestorsTabViewModel : BindableBase
{
    private readonly IInvestorsRepository repository;
    private ObservableCollection<Investor> _investors = new();

    public ObservableCollection<Investor> Investors
    {
        get => _investors;
        set => SetProperty(ref _investors, value);
    }

    public InvestorsTabViewModel(
        IInvestorsRepository repository,
        IEventAggregator eventAggregator)
    {
        this.repository = repository;

        eventAggregator.GetEvent<NextMonthRequestedEvent>()
            .Subscribe(investmentsSummary => CompileInvestorList(investmentsSummary).Await());
    }

    private async Task CompileInvestorList(NewInvestmentsSummary investmentsSummary)
    {
        List<Investor> investors = await GetAllNewInvestors(investmentsSummary);
        Investors.Clear();
        Investors.AddRange(investors.OrderByDescending(investor => investor.Investment));
    }

    private async Task<List<Investor>> GetAllNewInvestors(NewInvestmentsSummary investmentsSummary)
    {
        List<Investor> investors = investmentsSummary.NewInvestors.ToList();

        foreach (var investment in investmentsSummary.Reinvestments)
        {
            var investor = await repository.GetInvestorByIdAsync(investment.InvestorId);
            investors.Add(investor);
        }

        return investors;
    }
}
