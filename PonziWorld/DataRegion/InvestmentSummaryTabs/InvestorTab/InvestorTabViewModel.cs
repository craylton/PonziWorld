using PonziWorld.Core;
using PonziWorld.DataRegion.PerformanceHistoryTab;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.InvestorTab;

internal class InvestorTabViewModel : BindableSubscriberBase
{
    private readonly IInvestmentsRepository investmentsRepository;
    private ObservableCollection<HistoricalTransaction> _transactions = new();

    public ObservableCollection<HistoricalTransaction> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }

    public InvestorTabViewModel(
        IInvestmentsRepository investmentsRepository,
        IEventAggregator eventAggregator)
        : base(eventAggregator)
    {
        this.investmentsRepository = investmentsRepository;

        SubscribeToProcess(DisplayInvestor.Process, ShowInvestorAsync);
    }

    private async Task<InvestorDisplayedEventPayload> ShowInvestorAsync(DisplayInvestorCommandPayload payload)
    {
        Transactions.Clear();
        IEnumerable<Investment> investments = await investmentsRepository.GetInvestmentsByInvestorIdAsync(payload.Investor.Id);
        List<HistoricalTransaction> transactions = new();
        int firstInvestmentMonth = investments.Min(investment => investment.Month);
        int currentMonth = payload.interestRateHistory.Count();
        double cumulativeTotal = 0;

        for (int month = firstInvestmentMonth; month < currentMonth; month++)
        {
            double interestAmount = GetInterestAmount(month, payload.interestRateHistory, cumulativeTotal);

            if (interestAmount != 0)
            {
                cumulativeTotal += interestAmount;
                Transactions.Add(new(month, interestAmount, cumulativeTotal, TransactionType.Interest));
            }

            Investment? investment = investments.SingleOrDefault(investment => investment.Month == month);

            if (investment is not null)
            {
                cumulativeTotal += investment.Amount;
                Transactions.Add(new(month, investment.Amount, cumulativeTotal, TransactionType.Investment));
            }
        }

        return new();
    }

    private static double GetInterestAmount(
        int month,
        IEnumerable<MonthlyPerformance> interestRateHistory,
        double cumulativeTotal)
    {
        double interestRate = interestRateHistory.Single(interestRate => interestRate.Month == month).InterestRate;
        return cumulativeTotal * interestRate / 100;
    }
}
