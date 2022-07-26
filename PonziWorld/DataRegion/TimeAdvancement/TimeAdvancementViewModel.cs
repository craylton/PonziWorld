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

        NextMonthCommand = new(() => GoToNextMonthAsync().Await(), CanGoToNextMonth);
    }

    private async Task GoToNextMonthAsync()
    {
        CanAdvance = false;

        // apply % to existing investors and update satisfaction
        // update company stats (suspicion etc)
        // calculate and store results of company's investments

        NewInvestmentsSummary newInvestmentsSummary = await timeAdvancementCoordinator.GetNextMonthInvestmentsAsync();
        await timeAdvancementCoordinator.ApplyAsync(newInvestmentsSummary);
        eventAggregator.GetEvent<NewMonthInvestmentsGeneratedEvent>().Publish(newInvestmentsSummary);

        CanAdvance = true;
    }

    private bool CanGoToNextMonth() => CanAdvance;
}
