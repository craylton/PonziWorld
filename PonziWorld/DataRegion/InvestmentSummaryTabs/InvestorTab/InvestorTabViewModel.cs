using PonziWorld.Core;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.InvestorTab;

internal class InvestorTabViewModel : BindableSubscriberBase
{
    private Investor _investor;

    public Investor Investor
    {
        get => _investor;
        set => SetProperty(ref _investor, value);
    }

    public InvestorTabViewModel(
        IEventAggregator eventAggregator)
        : base(eventAggregator)
    {
        _investor = new(Guid.NewGuid(), "No investor selected", 0, 0, 0);
        SubscribeToProcess(DisplayInvestor.Process, ShowInvestor);
    }

    private InvestorDisplayedEventPayload ShowInvestor(DisplayInvestorCommandPayload payload)
    {
        Investor = payload.Investor;
        return new();
    }
}
