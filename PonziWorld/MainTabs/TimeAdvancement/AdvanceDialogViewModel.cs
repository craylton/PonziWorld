using PonziWorld.Core;
using Prism.Commands;
using Prism.Events;

namespace PonziWorld.MainTabs.TimeAdvancement;

internal class AdvanceDialogViewModel : BindableSubscriberBase
{
    public double _interestRate = 1d;

    public double InterestRate
    {
        get => _interestRate;
        set => SetProperty(ref _interestRate, value);
    }

    public bool IsSubmitted { get; private set; }

    public DelegateCommand SubmitCommand { get; }

    public AdvanceDialogViewModel(IEventAggregator eventAggregator)
        : base(eventAggregator) =>
        SubmitCommand = new(() => IsSubmitted = true);
}
