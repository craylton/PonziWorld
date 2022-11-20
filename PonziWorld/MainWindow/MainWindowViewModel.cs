using MahApps.Metro.Controls.Dialogs;
using PonziWorld.Core;
using PonziWorld.Events;
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
        SubscribeToProcess(AcquireClaimedInterest.Process, ClaimInterestRate);

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

    private ClaimedInterestAcquiredEventPayload ClaimInterestRate(AcquireClaimedInterestCommandPayload _) =>
        new(1d);

    private void OnGameLoaded() => IsGameLoaded = true;
}
