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
        eventAggregator.GetEvent<LoadGameRequestedEvent>().Subscribe(() => UpdateInvestorList().Await());
        eventAggregator.GetEvent<NewGameInitiatedEvent>().Subscribe(companyName => DeleteAllInvestorsAsync(companyName).Await());
        eventAggregator.GetEvent<NextMonthRequestedEvent>().Subscribe(() => AddToInvestorPoolAsync().Await());
    }

    private async Task DeleteAllInvestorsAsync(string _) =>
        await repository.DeleteAllInvestors();

    private async Task AddToInvestorPoolAsync()
    {
        var newInvestor = new Investor(Generator.GenerateRandomFullName(), random.Next(0, 100));
        await repository.AddInvestorAsync(newInvestor);
        await UpdateInvestorList();
    }

    private async Task UpdateInvestorList()
    {
        var investors = await repository.GetAllInvestorsAsync();
        Investors.Clear();
        Investors.AddRange(investors);
    }
}
