using PonziWorld.Core;
using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Sagas;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.TimeAdvancement;

internal class TimeAdvancementViewModel : BindableSubscriberBase
{
    private readonly AdvanceToNextMonthSaga advanceToNextMonthSaga;
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
        AdvanceToNextMonthSaga advanceToNextMonthSaga,
        ITimeAdvancementCoordinator timeAdvancementCoordinator,
        IEventAggregator eventAggregator)
        : base(eventAggregator)
    {
        this.advanceToNextMonthSaga = advanceToNextMonthSaga;
        this.timeAdvancementCoordinator = timeAdvancementCoordinator;
        this.eventAggregator = eventAggregator;

        NextMonthCommand = new(GoToNextMonth, CanGoToNextMonth);

        SubscribeToProcess(GenerateNewMonthInvestments.Process, GenerateNewMonthInvestmentsAsync);
        SubscribeToProcess(ApplyNewMonthInvestments.Process, ApplyNewMonthInvestmentsAsync);
    }

    private async Task<NewMonthInvestmentsAppliedEventPayload> ApplyNewMonthInvestmentsAsync(
        ApplyNewMonthInvestmentsCommandPayload payload)
    {
        await timeAdvancementCoordinator.ApplyAsync(payload.NewInvestmentsSummary);
        return new();
    }

    private async Task<NewMonthInvestmentsGeneratedEventPayload> GenerateNewMonthInvestmentsAsync(
        GenerateNewMonthInvestmentsCommandPayload _)
    {
        NewInvestmentsSummary newInvestmentsSummary = await timeAdvancementCoordinator.GetNextMonthInvestmentsAsync();
        return new(newInvestmentsSummary);
    }

    private void GoToNextMonth()
    {
        advanceToNextMonthSaga.Start();


        //CanAdvance = false;

        //// apply % to existing investors and update satisfaction
        //// update company stats (suspicion etc)
        //// calculate and store results of company's investments

        //NewInvestmentsSummary newInvestmentsSummary = await timeAdvancementCoordinator.GetNextMonthInvestmentsAsync();
        //await timeAdvancementCoordinator.ApplyAsync(newInvestmentsSummary);
        //eventAggregator.GetEvent<NewMonthInvestmentsGeneratedEvent>().Publish(newInvestmentsSummary);

        //CanAdvance = true;
    }

    private bool CanGoToNextMonth() => CanAdvance;
}
