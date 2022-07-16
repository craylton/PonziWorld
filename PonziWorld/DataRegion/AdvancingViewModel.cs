using PonziWorld.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace PonziWorld.DataRegion;

internal class AdvancingViewModel : BindableBase
{
    private readonly IEventAggregator eventAggregator;

    public DelegateCommand NextMonthCommand { get; private set; }

    public AdvancingViewModel(IEventAggregator eventAggregator)
    {
        this.eventAggregator = eventAggregator;
        NextMonthCommand = new(GoToNextMonth, CanGoToNextMonth);
    }

    private void GoToNextMonth() =>
        eventAggregator.GetEvent<NextMonthRequestedEvent>().Publish();

    private bool CanGoToNextMonth() => true;
}
