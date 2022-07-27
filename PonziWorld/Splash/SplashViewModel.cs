using PonziWorld.Company;
using PonziWorld.Events;
using PonziWorld.Sagas;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace PonziWorld.Splash;

internal class SplashViewModel : BindableBase
{
    private readonly LoadGameSaga loadGameSaga;
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
        LoadGameSaga loadGameSaga,
        ICompanyRepository repository,
        IEventAggregator eventAggregator)
    {
        this.loadGameSaga = loadGameSaga;
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

    private void LoadGame() => loadGameSaga.StartSaga();

    private void StartNewGame() =>
        eventAggregator.GetEvent<NewGameRequestedEvent>().Publish();

    private bool CanLoadGame() => SavedGameExists;

    private bool CanStartNewGame() => CanAccessDatabase;
}
