using PonziWorld.Core;
using PonziWorld.DataRegion.InvestmentsTab.Processes;
using PonziWorld.DataRegion.InvestmentSummaryTabs.DepositorsTab.Processes;
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
        SubscribeToProcess(RetrieveInvestmentsForLastMonth.Process, GetAllLastMonthInvestmentsAsync);
        SubscribeToProcess(ClearInvestments.Process, DeleteAllInvestmentsAsync);

        SubscribeToProcess(LoadDeposits.Process, LoadDepositsAsync);
        SubscribeToProcess(LoadDepositsForNewMonth.Process, LoadDepositsForNewMonthAsync);
    }

    private async Task<InvestmentsForLastMonthRetrievedEventPayload> GetAllLastMonthInvestmentsAsync(
        RetrieveInvestmentsForLastMonthCommandPayload payload)
    {
        IEnumerable<Investment> lastMonthInvestments = await investmentsRepository
            .GetInvestmentsByMonthAsync(payload.CurrentMonth - 1);

        return new(lastMonthInvestments);
    }

    private async Task<InvestmentsClearedEventPayload> DeleteAllInvestmentsAsync(ClearInvestmentsCommandPayload _)
    {
        await investmentsRepository.DeleteAllInvestmentsAsync();
        return new();
    }

    private async Task<DepositsLoadedEventPayload> LoadDepositsAsync(LoadDepositsCommandPayload payload)
    {
        IEnumerable<Investment> lastMonthDeposits = payload.LastMonthInvestments
            .Where(investment => investment.Amount > 0);

        IEnumerable<DetailedInvestment> deposits = await GetDetailedDepositsAsync(lastMonthDeposits);
        SetDepositsList(deposits);

        return new(deposits);
    }

    private async Task<DepositsForNewMonthLoadedEventPayload> LoadDepositsForNewMonthAsync(
        LoadDepositsForNewMonthCommandPayload payload)
    {
        IEnumerable<DetailedInvestment> deposits = await GetAllNewDepositsAsync(payload.NewInvestmentsSummary);
        SetDepositsList(deposits);
        return new(deposits);
    }

    private void SetDepositsList(IEnumerable<DetailedInvestment> deposits)
    {
        Deposits.Clear();
        Deposits.AddRange(deposits.OrderByDescending(deposit => deposit.InvestmentSize));
    }

    private async Task<IEnumerable<DetailedInvestment>> GetAllNewDepositsAsync(NewInvestmentsSummary investmentsSummary)
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
            detailedDeposits.Add(new DetailedInvestment(investor, deposit));
        }

        return detailedDeposits;
    }
}
