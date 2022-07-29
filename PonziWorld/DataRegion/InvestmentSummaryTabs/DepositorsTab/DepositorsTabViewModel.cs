using PonziWorld.Core;
using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.DepositorsTab;

internal class DepositorsTabViewModel : BindableSubscriberBase
{
    private readonly IInvestorsRepository investorsRepository;
    private readonly IInvestmentsRepository investmentsRepository;
    private ObservableCollection<DetailedInvestment> _deposits = new();

    public ObservableCollection<DetailedInvestment> Deposits
    {
        get => _deposits;
        set => SetProperty(ref _deposits, value);
    }

    public DepositorsTabViewModel(
        IInvestorsRepository investorsRepository,
        IInvestmentsRepository investmentsRepository,
        IEventAggregator eventAggregator)
        : base(eventAggregator)
    {
        this.investorsRepository = investorsRepository;
        this.investmentsRepository = investmentsRepository;

        // TODO: move these somewhere more appropriate
        SubscribeToProcess(ClearInvestments.Process, DeleteAllInvestmentsAsync);
        SubscribeToProcess(LoadInvestmentsForLastMonth.Process, LoadAllLastMonthInvestmentsAsync);

        SubscribeToProcess(LoadDeposits.Process, LoadDepositsAsync);

        eventAggregator.GetEvent<NewMonthInvestmentsGeneratedEvent>()
            .SubscribeAsync(CompileDepositListAsync);
    }

    private async Task<DepositsLoadedEventPayload> LoadDepositsAsync(LoadDepositsCommandPayload payload)
    {
        IEnumerable<Investment> lastMonthDeposits = payload.Investments
            .Where(investment => investment.Amount > 0);

        IEnumerable<DetailedInvestment> deposits = await GetDetailedDepositsAsync(lastMonthDeposits);
        SetDepositsList(deposits);

        return new();
    }

    private async Task<InvestmentsClearedEventPayload> DeleteAllInvestmentsAsync(ClearInvestmentsCommandPayload _)
    {
        await investmentsRepository.DeleteAllInvestmentsAsync();
        return new();
    }

    private async Task<InvestmentsForLastMonthLoadedEventPayload> LoadAllLastMonthInvestmentsAsync(
        LoadInvestmentsForLastMonthCommandPayload payload)
    {
        IEnumerable<Investment> lastMonthInvestments = await investmentsRepository
            .GetInvestmentsByMonthAsync(payload.CurrentMonth - 1);

        return new(lastMonthInvestments);
    }

    private async Task CompileDepositListAsync(NewInvestmentsSummary investmentsSummary)
    {
        IEnumerable<DetailedInvestment> deposits = await GetAllNewDepositsAsync(investmentsSummary);
        SetDepositsList(deposits);
    }

    private void SetDepositsList(IEnumerable<DetailedInvestment> deposits)
    {
        Deposits.Clear();
        Deposits.AddRange(deposits.OrderByDescending(deposit => deposit.InvestmentSize));
    }

    private async Task<IEnumerable<DetailedInvestment>> GetAllNewDepositsAsync(
        NewInvestmentsSummary investmentsSummary)
    {
        IEnumerable<DetailedInvestment> newInvestments = investmentsSummary.NewInvestors
            .Select(investor => new DetailedInvestment(investor.Name, investor.Investment, 0));

        IEnumerable<DetailedInvestment> reinvestments = await GetDetailedDepositsAsync(
            investmentsSummary.Reinvestments);

        return newInvestments.Concat(reinvestments);
    }

    private async Task<IEnumerable<DetailedInvestment>> GetDetailedDepositsAsync(IEnumerable<Investment> deposits)
    {
        List<DetailedInvestment> detailedDeposits = new();

        foreach (Investment deposit in deposits)
        {
            Investor investor = await investorsRepository.GetInvestorByIdAsync(deposit.InvestorId);
            detailedDeposits.Add(new DetailedInvestment(
                investor.Name,
                deposit.Amount,
                investor.Investment - deposit.Amount));
        }

        return detailedDeposits;
    }
}
