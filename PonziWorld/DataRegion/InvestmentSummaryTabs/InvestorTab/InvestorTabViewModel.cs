using PonziWorld.Core;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.InvestorTab;

internal class InvestorTabViewModel : BindableSubscriberBase
{
    private readonly IInvestmentsRepository investmentsRepository;
    private Investor _investor = new(Guid.NewGuid(), "No investor selected", 0, 0, 0);
    private ObservableCollection<Investment> _investments = new();

    public Investor Investor
    {
        get => _investor;
        set => SetProperty(ref _investor, value);
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
        Investments.AddRange(investments);
        return new();
    }
}
