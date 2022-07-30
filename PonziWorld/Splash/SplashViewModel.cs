using PonziWorld.Company;
using PonziWorld.Core;
using PonziWorld.Sagas;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;

namespace PonziWorld.Splash;

internal class SplashViewModel : BindableSubscriberBase
{
    private readonly LoadGameSaga loadGameSaga;
    private readonly StartNewGameSaga startNewGameSaga;
    private readonly ICompanyRepository repository;
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
        StartNewGameSaga startNewGameSaga,
        ICompanyRepository repository,
        IEventAggregator eventAggregator)
        : base(eventAggregator)
    {
        this.loadGameSaga = loadGameSaga;
        this.startNewGameSaga = startNewGameSaga;
        this.repository = repository;

        LoadCommand = new(LoadGame, CanLoadGame);
        NewGameCommand = new(StartNewGame, CanStartNewGame);

        SubscribeToProcess(TestDatabaseConnection.Process, TryConnectToDatabase);
    }

    private async Task<DatabaseConnectionTestedEventPayload> TryConnectToDatabase(
        TestDatabaseConnectionCommandPayload _)
    {
        try
        {
            SavedGameExists = await repository.GetCompanyExistsAsync();
            CanAccessDatabase = true;
        }
        catch
        {
            CanAccessDatabase = false;
        }

        return new(CanAccessDatabase);
    }

    private void LoadGame() => loadGameSaga.Start();

    private void StartNewGame() => startNewGameSaga.Start();

    private bool CanLoadGame() => SavedGameExists;

    private bool CanStartNewGame() => CanAccessDatabase;
}
