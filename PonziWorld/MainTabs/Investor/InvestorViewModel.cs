using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using PonziWorld.Core;
using PonziWorld.Investments;
using PonziWorld.MainTabs.Investor.Processes;
using PonziWorld.MainTabs.PerformanceHistory;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.MainTabs.Investor;

internal class InvestorViewModel : BindableSubscriberBase
{
    private readonly IInvestmentsRepository investmentsRepository;
    private ObservableCollection<HistoricalTransaction> _transactions = new();
    private ISeries[] _series = new ISeries[1];

    public ObservableCollection<HistoricalTransaction> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }

    public ISeries[] Series
    {
        get => _series;
        set => SetProperty(ref _series, value);
    }

    public InvestorViewModel(
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
        int currentMonth = payload.InterestRateHistory.Count();
        double cumulativeTotal = 0;
        List<ObservablePoint> dataPoints = new();

        for (int month = firstInvestmentMonth; month < currentMonth; month++)
        {
            double interestAmount = GetInterestAmount(month, payload.InterestRateHistory, cumulativeTotal);

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

            dataPoints.Add(new ObservablePoint(month, cumulativeTotal));
        }

        SetInvestmentDataSeries(dataPoints);

        return new();
    }

    private void SetInvestmentDataSeries(List<ObservablePoint> dataPoints) =>
        Series = new ISeries[]
        {
            new LineSeries<ObservablePoint>
            {
                Values = dataPoints,
                Fill = null,
                GeometrySize = 4,
                LineSmoothness = 0.2,
            }
        };

    private static double GetInterestAmount(
        int month,
        IEnumerable<MonthlyPerformance> interestRateHistory,
        double cumulativeTotal)
    {
        double interestRate = interestRateHistory.Single(interestRate => interestRate.Month == month).InterestRate;
        return cumulativeTotal * interestRate / 100;
    }
}
