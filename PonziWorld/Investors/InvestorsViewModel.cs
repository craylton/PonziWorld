using NameGenerator;
using PonziWorld.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PonziWorld.Investors;

internal class InvestorsViewModel : BindableBase
{
    private readonly Random random = new();
    private readonly IInvestorsRepository repository;
    private readonly IInvestorPool investorPool;
    private ObservableCollection<Investor> _investors = new();

    public ObservableCollection<Investor> Investors
    {
        get => _investors;
        set => SetProperty(ref _investors, value);
    }

    public InvestorsViewModel(
        IInvestorsRepository repository,
        IInvestorPool investorPool,
        IEventAggregator eventAggregator)
    {
        this.repository = repository;
        this.investorPool = investorPool;
        eventAggregator.GetEvent<LoadGameRequestedEvent>().Subscribe(() => UpdateInvestorList().Await());
        eventAggregator.GetEvent<NewGameInitiatedEvent>().Subscribe(companyName => DeleteAllInvestorsAsync(companyName).Await());
        eventAggregator.GetEvent<NextMonthRequestedEvent>().Subscribe(() => AddToInvestorPoolAsync().Await());
    }

    private async Task DeleteAllInvestorsAsync(string _) =>
        await repository.DeleteAllInvestors();

    private async Task AddToInvestorPoolAsync()
    {
        await investorPool.AddInvestorsToPool();
        await UpdateInvestorList();
    }

    private async Task UpdateInvestorList()
    {
        var investors = await repository.GetAllActiveInvestorsAsync();
        Investors.Clear();
        Investors.AddRange(investors);
    }
}
