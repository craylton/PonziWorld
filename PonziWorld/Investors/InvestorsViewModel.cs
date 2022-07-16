using PonziWorld.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.Investors;

internal class InvestorsViewModel : BindableBase
{
    private readonly Random random = new();
    private readonly IInvestorsRepository repository;
    private ObservableCollection<Investor> _existingInvestors = new();

    public ObservableCollection<Investor> Investors
    {
        get => _existingInvestors;
        set => SetProperty(ref _existingInvestors, value);
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
        var newInvestor = new Investor(GenerateRandomName(), random.Next(0, 100));
        await repository.AddInvestorAsync(newInvestor);
        await UpdateInvestorList();
    }

    private async Task UpdateInvestorList()
    {
        var investors = await repository.GetAllInvestorsAsync();
        Investors.Clear();
        Investors.AddRange(investors);
    }

    // https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
    private string GenerateRandomName() =>
        new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 8)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
}
