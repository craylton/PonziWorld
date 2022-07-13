using PonziWorld.Bootstrapping;
using PonziWorld.DataRegion;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.ExistingInvestors;

internal class ExistingInvestorsViewModel : BindableBase
{
    private Random random = new();
    private readonly IInvestorsRepository repository;
    private ObservableCollection<ExistingInvestor> _existingInvestors = new();

    public ObservableCollection<ExistingInvestor> Investors
    {
        get => _existingInvestors;
        set => SetProperty(ref _existingInvestors, value);
    }

    public ExistingInvestorsViewModel(
        IInvestorsRepository repository,
        IEventAggregator eventAggregator)
    {
        this.repository = repository;
        eventAggregator.GetEvent<MainWindowInitialisedEvent>().Subscribe(Initialise);
        eventAggregator.GetEvent<NextMonthRequestedEvent>().Subscribe(AddToInvestorPool);
    }

    private async void Initialise()
    {
        await UpdateInvestorList();
    }

    private async void AddToInvestorPool()
    {
        await repository.AddInvestorAsync(new(GenerateRandomName(), random.Next(0, 100)));
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
