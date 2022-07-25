using PonziWorld.Events;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PonziWorld.Investments.Investors;

internal class InvestorsViewModel : BindableBase
{
    private readonly IInvestorsRepository repository;
    private ObservableCollection<Investor> _investors = new();

    public ObservableCollection<Investor> Investors
    {
        get => _investors;
        set => SetProperty(ref _investors, value);
    }

    public InvestorsViewModel(
        IInvestorsRepository repository,
        IEventAggregator eventAggregator)
    {
        this.repository = repository;

        eventAggregator.GetEvent<LoadGameRequestedEvent>()
            .Subscribe(() => UpdateInvestorList().Await());

        eventAggregator.GetEvent<NewGameInitiatedEvent>()
            .Subscribe(_ => DeleteAllInvestorsAsync().Await());

        eventAggregator.GetEvent<NextMonthRequestedEvent>()
            .Subscribe(_ => UpdateInvestorList().Await());
    }

    private async Task DeleteAllInvestorsAsync() =>
        await repository.DeleteAllInvestors();

    private async Task UpdateInvestorList()
    {
        IEnumerable<Investor> investors = await repository.GetAllActiveInvestorsAsync();
        Investors.Clear();
        Investors.AddRange(investors);
    }
}
