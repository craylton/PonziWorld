using MahApps.Metro.Controls.Dialogs;
using PonziWorld.Events;
using Prism.Events;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace PonziWorld.MainWindow;

internal class MainWindowViewModel : BindableBase
{
    private readonly IEventAggregator eventAggregator;
    private readonly IDialogCoordinator dialogCoordinator;
    private bool _isGameLoaded = false;

    public bool IsGameLoaded
    {
        get => _isGameLoaded;
        set => SetProperty(ref _isGameLoaded, value);
    }

    public MainWindowViewModel(
        IEventAggregator eventAggregator,
        IDialogCoordinator dialogCoordinator)
    {
        this.eventAggregator = eventAggregator;
        this.dialogCoordinator = dialogCoordinator;

        eventAggregator.GetEvent<NewGameRequestedEvent>()
            .Subscribe(() => StartNewGameAsync().Await());

        eventAggregator.GetEvent<LoadGameRequestedEvent>()
            .Subscribe(OnGameLoaded);
    }

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
