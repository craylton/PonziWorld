using MahApps.Metro.Controls.Dialogs;
using PonziWorld.Events;
using PonziWorld.Sagas;
using Prism.Events;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace PonziWorld.MainWindow;

internal class MainWindowViewModel : BindableBase
{
    private readonly StartApplicationSaga startApplicationSaga;
    private readonly IEventAggregator eventAggregator;
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
    {
        this.startApplicationSaga = startApplicationSaga;
        this.eventAggregator = eventAggregator;
        this.dialogCoordinator = dialogCoordinator;

        eventAggregator.GetEvent<MainWindowInitialisedEvent>()
            .Subscribe(InitialiseApplication);

        eventAggregator.GetEvent<NewGameRequestedEvent>()
            .Subscribe(() => StartNewGameAsync().Await());

        eventAggregator.GetEvent<LoadGameCompletedEvent>()
            .Subscribe(OnGameLoaded);
    }

    private void InitialiseApplication() => startApplicationSaga.StartSaga();

    private void OnGameLoaded() => IsGameLoaded = true;

    private async Task StartNewGameAsync()
    {
        string? companyName = await dialogCoordinator
            .ShowInputAsync(this, "Create a new game", "Enter the name of your company");

        if (companyName is null)
            return;

        eventAggregator.GetEvent<NewGameInitiatedEvent>().Publish(companyName);
        IsGameLoaded = true;
    }
}
