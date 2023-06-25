using MahApps.Metro.Controls.Dialogs;
using PonziWorld.Core;
using PonziWorld.Events;
using PonziWorld.MainTabs.TimeAdvancement;
using PonziWorld.MainWindow.Processes;
using PonziWorld.Sagas;
using Prism.Events;
using System.Threading.Tasks;

namespace PonziWorld.MainWindow;

internal class MainWindowViewModel : BindableSubscriberBase
{
    private readonly StartApplicationSaga startApplicationSaga;
    private readonly IDialogCoordinator dialogCoordinator;
    private bool _isGameLoaded = false;

    public bool IsGameLoaded
    {
        get => _isGameLoaded;
        set => SetProperty(ref _isGameLoaded, value);
    }

    public MainWindowViewModel(
        StartApplicationSaga startApplicationSaga,
        IEventAggregator eventAggregator,
        IDialogCoordinator dialogCoordinator)
        : base(eventAggregator)
    {
        this.startApplicationSaga = startApplicationSaga;
        this.dialogCoordinator = dialogCoordinator;

        eventAggregator.GetEvent<MainWindowInitialisedEvent>()
            .Subscribe(InitialiseApplication);

        SubscribeToProcess(AcquireNewGameSettings.Process, StartNewGameAsync);
        SubscribeToProcess(ExitMenu.Process, OnGameLoaded);
        SubscribeToProcess(RetrieveClaimedInterest.Process, ClaimInterestRateAsync);

        eventAggregator.GetEvent<LoadGameCompletedEvent>()
            .Subscribe(OnGameLoaded);
    }

    private void InitialiseApplication() => startApplicationSaga.Start();

    private async Task<NewGameSettingsAcquiredEventPayload> StartNewGameAsync(AcquireNewGameSettingsCommandPayload _)
    {
        string? companyName = await dialogCoordinator
            .ShowInputAsync(this, "Create a new game", "Enter the name of your company");

        if (companyName is null)
            return new(string.Empty, true);

        return new(companyName, false);
    }

    private MenuExitedEventPayload OnGameLoaded(ExitMenuCommandPayload _)
    {
        OnGameLoaded();
        return new();
    }

    private async Task<ClaimedInterestRetrievedEventPayload> ClaimInterestRateAsync(
        RetrieveClaimedInterestCommandPayload _)
    {
        AdvanceDialog dialog = new();
        await dialogCoordinator.ShowMetroDialogAsync(this, dialog);

        while (true)
        {
            if (dialog.IsSubmitted())
                break;

            await Task.Delay(100);
        }

        await dialogCoordinator.HideMetroDialogAsync(this, dialog);
        return new(dialog.InterestRate());
    }

    private void OnGameLoaded() => IsGameLoaded = true;
}
