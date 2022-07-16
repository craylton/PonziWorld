using PonziWorld.Company;
using PonziWorld.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace PonziWorld.Splash;

internal class SplashViewModel : BindableBase
{
    private readonly ICompanyRepository repository;
    private readonly IEventAggregator eventAggregator;
    private bool savedGameExists;

    public bool SavedGameExists
    {
        get => savedGameExists;
        set
        {
            SetProperty(ref savedGameExists, value);
            LoadCommand.RaiseCanExecuteChanged();
        }
    }

    public DelegateCommand LoadCommand { get; private set; }
    public DelegateCommand NewGameCommand { get; private set; }

    public SplashViewModel(
        ICompanyRepository repository,
        IEventAggregator eventAggregator)
    {
        this.repository = repository;
        this.eventAggregator = eventAggregator;
        LoadCommand = new(LoadGame, CanLoadGame);
        NewGameCommand = new(StartNewGame);
        eventAggregator.GetEvent<MainWindowInitialisedEvent>().Subscribe(() => InitialiseAsync().Await());
    }

    private async Task InitialiseAsync() =>
        SavedGameExists = await repository.GetCompanyExistsAsync();

    private void LoadGame() =>
        eventAggregator.GetEvent<LoadGameRequestedEvent>().Publish();

    private void StartNewGame() =>
        eventAggregator.GetEvent<NewGameRequestedEvent>().Publish();

    private bool CanLoadGame() => SavedGameExists;
}
