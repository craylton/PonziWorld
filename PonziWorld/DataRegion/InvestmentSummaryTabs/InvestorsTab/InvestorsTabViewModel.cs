using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.InvestorsTab;

internal class InvestorsTabViewModel : BindableBase
{
    private readonly IInvestorsRepository repository;
    private ObservableCollection<DetailedInvestment> _investments = new();

    public ObservableCollection<DetailedInvestment> Investments
    {
        get => _investments;
        set => SetProperty(ref _investments, value);
    }

    public InvestorsTabViewModel(
        IInvestorsRepository repository,
        IEventAggregator eventAggregator)
    {
        this.repository = repository;

        eventAggregator.GetEvent<NextMonthRequestedEvent>()
            .Subscribe(investmentsSummary => CompileInvestmentList(investmentsSummary).Await());
    }

    private async Task CompileInvestmentList(NewInvestmentsSummary investmentsSummary)
    {
        List<DetailedInvestment> investments = await GetAllNewInvestments(investmentsSummary);
        Investments.Clear();
        Investments.AddRange(investments.OrderByDescending(investment => investment.InvestmentSize));
    }

    private async Task<List<DetailedInvestment>> GetAllNewInvestments(NewInvestmentsSummary investmentsSummary)
    {
        List<DetailedInvestment> investments = investmentsSummary.NewInvestors
            .Select(investor => new DetailedInvestment(investor.Name, investor.Investment, 0))
            .ToList();

        foreach (Investment reinvestment in investmentsSummary.Reinvestments)
        {
            Investor investor = await repository.GetInvestorByIdAsync(reinvestment.InvestorId);
            investments.Add(new DetailedInvestment(
                investor.Name,
                reinvestment.Amount,
                investor.Investment - reinvestment.Amount));
        }

        return investments;
    }
}
