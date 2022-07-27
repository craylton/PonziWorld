using PonziWorld.Core;
using PonziWorld.Events;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PonziWorld.Investments.Investors;

internal class InvestorsViewModel : BindableSubscriberBase
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
        : base(eventAggregator)
    {
        this.repository = repository;
        this.eventAggregator = eventAggregator;

        SubscribeToProcess(LoadInvestors.Process, UpdateInvestorListAsync);
        eventAggregator.GetEvent<LoadInvestorsCommand>()
            .SubscribeAsync(UpdateInvestorListAsync);

        eventAggregator.GetEvent<NewGameInitiatedEvent>()
            .SubscribeAsync(DeleteAllInvestorsAsync);

        eventAggregator.GetEvent<NewMonthInvestmentsGeneratedEvent>()
            .SubscribeAsync(UpdateInvestorListAsync);
    }

    private async Task DeleteAllInvestorsAsync(string _) =>
        await repository.DeleteAllInvestorsAsync();

    private async Task<InvestorsLoadedEventPayload> UpdateInvestorListAsync(LoadInvestorsCommandPayload _)
    {
        IEnumerable<Investor> investors = await repository.GetAllActiveInvestorsAsync();
        Investors.Clear();
        Investors.AddRange(investors);
        return new();
    }

    private async Task UpdateInvestorListAsync(NewInvestmentsSummary _)
    {
        IEnumerable<Investor> investors = await repository.GetAllActiveInvestorsAsync();
        Investors.Clear();
        Investors.AddRange(investors);
        eventAggregator.GetEvent<InvestorsLoadedEvent>().Publish(new());
    }
}
