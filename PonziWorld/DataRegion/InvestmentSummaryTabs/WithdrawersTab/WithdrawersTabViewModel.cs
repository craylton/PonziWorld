using PonziWorld.Core;
using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.WithdrawersTab;

internal class WithdrawersTabViewModel : BindableSubscriberBase
{
    private readonly IInvestorsRepository investorsRepository;
    private ObservableCollection<DetailedInvestment> _withdrawals = new();

    public ObservableCollection<DetailedInvestment> Withdrawals
    {
        get => _withdrawals;
        set => SetProperty(ref _withdrawals, value);
    }

    public WithdrawersTabViewModel(
        IInvestorsRepository investorsRepository,
        IEventAggregator eventAggregator)
        : base(eventAggregator)
    {
        this.investorsRepository = investorsRepository;

        SubscribeToProcess(LoadWithdrawals.Process, LoadWithdrawalsAsync);
        SubscribeToProcess(LoadWithdrawalsForNewMonth.Process, LoadWithdrawalsForNewMonthAsync);

        //eventAggregator.GetEvent<NewMonthInvestmentsGeneratedEvent>()
        //    .SubscribeAsync(CompileWithdrawalListAsync);
    }

    private async Task<WithdrawalsForNewMonthLoadedEventPayload> LoadWithdrawalsForNewMonthAsync(LoadWithdrawalsForNewMonthCommandPayload payload)
    {
        IEnumerable<DetailedInvestment> withdrawals = await GetDetailedWithdrawalsAsync(payload.NewInvestmentsSummary.Withdrawals);
        SetWithdrawalsList(withdrawals);
        return new();
    }

    private async Task<WithdrawalsLoadedEventPayload> LoadWithdrawalsAsync(LoadWithdrawalsCommandPayload payload)
    {
        IEnumerable<Investment> lastMonthWithdrawals = payload.LastMonthInvestments
            .Where(investment => investment.Amount < 0);

        IEnumerable<DetailedInvestment> investments = await GetDetailedWithdrawalsAsync(lastMonthWithdrawals);
        SetWithdrawalsList(investments);

        return new();
    }

    private async Task CompileWithdrawalListAsync(NewInvestmentsSummary investmentsSummary)
    {
        IEnumerable<DetailedInvestment> withdrawals = await GetDetailedWithdrawalsAsync(investmentsSummary.Withdrawals);
        SetWithdrawalsList(withdrawals);
    }

    private void SetWithdrawalsList(IEnumerable<DetailedInvestment> withdrawals)
    {
        Withdrawals.Clear();
        Withdrawals.AddRange(withdrawals.OrderByDescending(withdrawal => withdrawal.InvestmentSize));
    }

    private async Task<IEnumerable<DetailedInvestment>> GetDetailedWithdrawalsAsync(IEnumerable<Investment> withdrawals)
    {
        List<DetailedInvestment> detailedInvestments = new();

        foreach (Investment withdrawal in withdrawals)
        {
            Investor investor = await investorsRepository.GetInvestorByIdAsync(withdrawal.InvestorId);
            detailedInvestments.Add(new DetailedInvestment(investor, withdrawal));
        }

        return detailedInvestments;
    }
}
