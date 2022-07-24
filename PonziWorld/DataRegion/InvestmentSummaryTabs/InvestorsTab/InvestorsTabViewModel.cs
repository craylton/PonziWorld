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
    private ObservableCollection<DetailedInvestment> _investors = new();

    public ObservableCollection<DetailedInvestment> Investors
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
        List<DetailedInvestment> investments = await GetAllNewInvestments(investmentsSummary);
        Investors.Clear();
        Investors.AddRange(investments.OrderByDescending(investor => investor.InvestmentSize));
    }

    private async Task<List<DetailedInvestment>> GetAllNewInvestments(NewInvestmentsSummary investmentsSummary)
    {
        List<DetailedInvestment> investments = investmentsSummary.NewInvestors
            .Select(investor => new DetailedInvestment(investor.Name, investor.Investment, 0))
            .ToList();

        foreach (Investment reinvestment in investmentsSummary.Reinvestments)
        {
            var investor = await repository.GetInvestorByIdAsync(reinvestment.InvestorId);
            investments.Add(new DetailedInvestment(
                investor.Name,
                reinvestment.Amount,
                investor.Investment - reinvestment.Amount));
        }

        return investments;
    }
}
