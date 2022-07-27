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
    private readonly IInvestorsRepository investorsRepository;
    private readonly IInvestmentsRepository investmentsRepository;
    private readonly IEventAggregator eventAggregator;
    private ObservableCollection<DetailedInvestment> _investments = new();

    public ObservableCollection<DetailedInvestment> Investments
    {
        get => _investments;
        set => SetProperty(ref _investments, value);
    }

    public InvestorsTabViewModel(
        IInvestorsRepository investorsRepository,
        IInvestmentsRepository investmentsRepository,
        IEventAggregator eventAggregator)
    {
        this.investorsRepository = investorsRepository;
        this.investmentsRepository = investmentsRepository;
        this.eventAggregator = eventAggregator;

        // TODO: move this somewhere more appropriate
        eventAggregator.GetEvent<NewGameInitiatedEvent>()
            .Subscribe(_ => DeleteAllInvestmentsAsync().Await());

        eventAggregator.GetEvent<LoadInvestmentsForLastMonthCommand>()
            .Subscribe(payload=> LoadAllLastMonthInvestmentsAsync(payload).Await());

        eventAggregator.GetEvent<LoadDepositsCommand>()
            .Subscribe(payload => LoadInvestmentsAsync(payload).Await());

        eventAggregator.GetEvent<NewMonthInvestmentsGeneratedEvent>()
            .Subscribe(investmentsSummary => CompileInvestmentListAsync(investmentsSummary).Await());
    }

    private async Task LoadInvestmentsAsync(LoadDepositsCommandPayload payload)
    {
        IEnumerable<Investment> lastMonthInvestments = payload.Investments
            .Where(investment => investment.Amount > 0);

        IEnumerable<DetailedInvestment> investments = await GetDetailedInvestmentsAsync(lastMonthInvestments);
        SetInvestmentsList(investments);

        eventAggregator.GetEvent<DepositsLoadedEvent>().Publish(new());
    }

    private async Task DeleteAllInvestmentsAsync() =>
        await investmentsRepository.DeleteAllInvestmentsAsync();

    private async Task LoadAllLastMonthInvestmentsAsync(LoadInvestmentsForLastMonthCommandPayload payload)
    {
        IEnumerable<Investment> lastMonthInvestments = await investmentsRepository
            .GetInvestmentsByMonthAsync(payload.Month - 1);

        eventAggregator.GetEvent<InvestmentsForLastMonthLoadedEvent>()
            .Publish(new(lastMonthInvestments));
    }

    private async Task CompileInvestmentListAsync(NewInvestmentsSummary investmentsSummary)
    {
        IEnumerable<DetailedInvestment> investments = await GetAllNewInvestmentsAsync(investmentsSummary);
        SetInvestmentsList(investments);
    }

    private void SetInvestmentsList(IEnumerable<DetailedInvestment> investments)
    {
        Investments.Clear();
        Investments.AddRange(investments.OrderByDescending(investment => investment.InvestmentSize));
    }

    private async Task<IEnumerable<DetailedInvestment>> GetAllNewInvestmentsAsync(NewInvestmentsSummary investmentsSummary)
    {
        IEnumerable<DetailedInvestment> investments = investmentsSummary.NewInvestors
            .Select(investor => new DetailedInvestment(investor.Name, investor.Investment, 0));

        IEnumerable<DetailedInvestment> reinvestments = await GetDetailedInvestmentsAsync(investmentsSummary.Reinvestments);
        return investments.Concat(reinvestments);
    }

    private async Task<IEnumerable<DetailedInvestment>> GetDetailedInvestmentsAsync(IEnumerable<Investment> investments)
    {
        List<DetailedInvestment> detailedInvestments = new();

        foreach (Investment investment in investments)
        {
            Investor investor = await investorsRepository.GetInvestorByIdAsync(investment.InvestorId);
            detailedInvestments.Add(new DetailedInvestment(
                investor.Name,
                investment.Amount,
                investor.Investment - investment.Amount));
        }

        return detailedInvestments;
    }
}
