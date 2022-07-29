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

        SubscribeToProcess(LoadInvestors.Process, UpdateInvestorListAsync);
        SubscribeToProcess(ClearInvestors.Process, DeleteAllInvestorsAsync);

        eventAggregator.GetEvent<NewMonthInvestmentsGeneratedEvent>()
            .SubscribeAsync(UpdateInvestorListAsync);
    }

    private async Task<InvestorsClearedEventPayload> DeleteAllInvestorsAsync(ClearInvestorsCommandPayload _)
    {
        await repository.DeleteAllInvestorsAsync();
        return new();
    }

    private async Task<InvestorsLoadedEventPayload> UpdateInvestorListAsync(LoadInvestorsCommandPayload _)
    {
        await LoadInvestorsAsync();
        return new();
    }

    private async Task UpdateInvestorListAsync(NewInvestmentsSummary _) =>
        await LoadInvestorsAsync();

    private async Task LoadInvestorsAsync()
    {
        IEnumerable<Investor> investors = await repository.GetAllActiveInvestorsAsync();
        Investors.Clear();
        Investors.AddRange(investors);
    }
}
