using PonziWorld.Company;
using PonziWorld.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;

namespace PonziWorld.Splash;

internal class SplashViewModel : BindableBase
{
    private readonly ICompanyRepository repository;
    private readonly IEventAggregator eventAggregator;
    private bool _savedGameExists = false;
    private bool _canAccessDatabase = false;

    public bool SavedGameExists
    {
        get => _savedGameExists;
        set
        {
            SetProperty(ref _savedGameExists, value);
            LoadCommand.RaiseCanExecuteChanged();
        }
    }

    public bool CanAccessDatabase
    {
        get => _canAccessDatabase;
        set
        {
            SetProperty(ref _canAccessDatabase, value);
            NewGameCommand.RaiseCanExecuteChanged();
        }
    }

    public DelegateCommand LoadCommand { get; }
    public DelegateCommand NewGameCommand { get; }

    public SplashViewModel(
        ICompanyRepository repository,
        IEventAggregator eventAggregator)
    {
        this.repository = repository;
        this.eventAggregator = eventAggregator;

        LoadCommand = new(LoadGame, CanLoadGame);
        NewGameCommand = new(StartNewGame, CanStartNewGame);

        eventAggregator.GetEvent<MainWindowInitialisedEvent>()
            .Subscribe(() => InitialiseAsync().Await());
    }

    private async Task InitialiseAsync()
    {
        SavedGameExists = await repository.GetCompanyExistsAsync();
        CanAccessDatabase = true;
    }

    private void LoadGame() =>
        eventAggregator.GetEvent<LoadGameRequestedEvent>().Publish();

    private void StartNewGame() =>
        eventAggregator.GetEvent<NewGameRequestedEvent>().Publish();

    private bool CanLoadGame() => SavedGameExists;

    private bool CanStartNewGame() => CanAccessDatabase;
}
