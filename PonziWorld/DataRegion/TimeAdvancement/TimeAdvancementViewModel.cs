using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.TimeAdvancement;

internal class TimeAdvancementViewModel : BindableBase
{
    private readonly ITimeAdvancementCoordinator timeAdvancementCoordinator;
    private readonly IEventAggregator eventAggregator;
    private bool _canAdvance = true;

    public bool CanAdvance
    {
        get => _canAdvance;
        set
        {
            SetProperty(ref _canAdvance, value);
            NextMonthCommand.RaiseCanExecuteChanged();
        }
    }

    public DelegateCommand NextMonthCommand { get; }

    public TimeAdvancementViewModel(
        ITimeAdvancementCoordinator timeAdvancementCoordinator,
        IEventAggregator eventAggregator)
    {
        this.timeAdvancementCoordinator = timeAdvancementCoordinator;
        this.eventAggregator = eventAggregator;

        NextMonthCommand = new(() => GoToNextMonth().Await(), CanGoToNextMonth);
    }

    private async Task GoToNextMonth()
    {
        CanAdvance = false;

        NewInvestmentsSummary newInvestmentsSummary = await timeAdvancementCoordinator.GetNextMonthInvestmentsAsync();
        await timeAdvancementCoordinator.ApplyAsync(newInvestmentsSummary);
        eventAggregator.GetEvent<NextMonthRequestedEvent>().Publish(newInvestmentsSummary);

        CanAdvance = true;
    }

    private bool CanGoToNextMonth() => CanAdvance;
}
