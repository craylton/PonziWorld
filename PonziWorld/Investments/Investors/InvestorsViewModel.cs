using PonziWorld.Core;
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
    private readonly IEventAggregator eventAggregator;
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
        this.eventAggregator = eventAggregator;

        eventAggregator.GetEvent<LoadInvestorsCommand>()
            .SubscribeAsync(UpdateInvestorListAsync);

        eventAggregator.GetEvent<NewGameInitiatedEvent>()
            .SubscribeAsync(DeleteAllInvestorsAsync);

        eventAggregator.GetEvent<NewMonthInvestmentsGeneratedEvent>()
            .SubscribeAsync(UpdateInvestorListAsync);
    }

    private async Task DeleteAllInvestorsAsync(string _) =>
        await repository.DeleteAllInvestorsAsync();

    private async Task UpdateInvestorListAsync(LoadInvestorsCommandPayload _) =>
        await UpdateInvestorListAsync();

    private async Task UpdateInvestorListAsync(NewInvestmentsSummary _) =>
        await UpdateInvestorListAsync();

    private async Task UpdateInvestorListAsync()
    {
        IEnumerable<Investor> investors = await repository.GetAllActiveInvestorsAsync();
        Investors.Clear();
        Investors.AddRange(investors);
        eventAggregator.GetEvent<InvestorsLoadedEvent>().Publish(new());
    }
}
