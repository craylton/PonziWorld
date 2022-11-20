using PonziWorld.Core;
using PonziWorld.DataRegion.TimeAdvancement.Processes;
using PonziWorld.Events;
using PonziWorld.Sagas;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.TimeAdvancement;

internal class TimeAdvancementViewModel : BindableSubscriberBase
{
    private readonly AdvanceToNextMonthSaga advanceToNextMonthSaga;
    private readonly ITimeAdvancementCoordinator timeAdvancementCoordinator;
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
        AdvanceToNextMonthSaga advanceToNextMonthSaga,
        ITimeAdvancementCoordinator timeAdvancementCoordinator,
        IEventAggregator eventAggregator)
        : base(eventAggregator)
    {
        this.advanceToNextMonthSaga = advanceToNextMonthSaga;
        this.timeAdvancementCoordinator = timeAdvancementCoordinator;

        NextMonthCommand = new(GoToNextMonth, CanGoToNextMonth);

        SubscribeToProcess(GenerateNewMonthInvestments.Process, GetNewMonthInvestments);
        SubscribeToProcess(ApplyNewMonthInvestments.Process, ApplyNewMonthInvestmentsAsync);

        eventAggregator.GetEvent<AdvanceToNextMonthStartedEvent>().Subscribe(() => CanAdvance = false);
        eventAggregator.GetEvent<AdvanceToNextMonthCompletedEvent>().Subscribe(() => CanAdvance = true);
    }

    private NewMonthInvestmentsGeneratedEventPayload GetNewMonthInvestments(
        GenerateNewMonthInvestmentsCommandPayload payload) =>
        new(timeAdvancementCoordinator.GetNextMonthInvestments(
            payload.Company,
            payload.Investors));

    private async Task<NewMonthInvestmentsAppliedEventPayload> ApplyNewMonthInvestmentsAsync(
        ApplyNewMonthInvestmentsCommandPayload payload)
    {
        await timeAdvancementCoordinator.ApplyAsync(payload.NewInvestmentsSummary);
        return new();
    }

    private void GoToNextMonth() => advanceToNextMonthSaga.Start();

    private bool CanGoToNextMonth() => CanAdvance;
}
