using PonziWorld.Core;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.InvestorTab;

internal class InvestorTabViewModel : BindableSubscriberBase
{
    private readonly IInvestmentsRepository investmentsRepository;
    private Investor _investor = new(Guid.NewGuid(), "No investor selected", 0, 0, 0);
    private ObservableCollection<HistoricalTransaction> _transactions = new();
    private ObservableCollection<Investment> _investments = new();

    public Investor Investor
    {
        get => _investor;
        set => SetProperty(ref _investor, value);
    }

    public ObservableCollection<HistoricalTransaction> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }

    public ObservableCollection<Investment> Investments
    {
        get => _investments;
        set => SetProperty(ref _investments, value);
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
        Investor = payload.Investor;
        IEnumerable<Investment> investments = await investmentsRepository.GetInvestmentsByInvestorIdAsync(Investor.Id);

        Investments.Clear();
        Transactions.Clear();

        Investments.AddRange(investments);
        List<HistoricalTransaction>? transactions = new();
        int firstInvestmentMonth = investments.Min(investment => investment.Month);
        double cumulativeTotal = 0;

        for (int month = firstInvestmentMonth; month < payload.interestRateHistory.Count(); month++)
        {
            double interestRate = payload.interestRateHistory
                .Single(interestRate => interestRate.Month == month).InterestRate / 100;
            double interestAmount = cumulativeTotal * interestRate;

            if (interestAmount > 0)
            {
                cumulativeTotal += interestAmount;

                transactions.Add(
                    new HistoricalTransaction(
                        month,
                        interestAmount,
                        cumulativeTotal,
                        TransactionType.Interest));
            }

            var investment = investments.SingleOrDefault(investment => investment.Month == month);

            if (investment is not null)
            {
                cumulativeTotal += investment.Amount;

                transactions.Add(
                    new HistoricalTransaction(
                        month,
                        investment.Amount,
                        cumulativeTotal,
                        TransactionType.Investment));
            }
        }

        Transactions.AddRange(transactions);

        return new();
    }
}
