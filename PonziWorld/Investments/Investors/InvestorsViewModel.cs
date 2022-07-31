using PonziWorld.Core;
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

        SubscribeToProcess(RetrieveInvestors.Process, GetAllInvestorsAsync);
        SubscribeToProcess(LoadInvestors.Process, UpdateInvestorListAsync);
        SubscribeToProcess(ClearInvestors.Process, DeleteAllInvestorsAsync);
        SubscribeToProcess(ApplyNewInterestRate.Process, OnNewInterestDeclarationAsync);
    }

    private async Task<InvestorsRetrievedEventPayload> GetAllInvestorsAsync(RetrieveInvestorsCommandPayload _) =>
        new(await repository.GetAllInvestorsAsync());

    private async Task<InvestorsLoadedEventPayload> UpdateInvestorListAsync(LoadInvestorsCommandPayload _)
    {
        IEnumerable<Investor> investors = await repository.GetAllActiveInvestorsAsync();
        Investors.Clear();
        Investors.AddRange(investors);
        return new(investors);
    }

    private async Task<InvestorsClearedEventPayload> DeleteAllInvestorsAsync(ClearInvestorsCommandPayload _)
    {
        await repository.DeleteAllInvestorsAsync();
        return new();
    }

    private async Task<NewInterestRateAppliedEventPayload> OnNewInterestDeclarationAsync(
        ApplyNewInterestRateCommandPayload incomingPayload) =>
        // TODO: Multiply all investment amounts by incomingPayload.ClaimedInterest
        new(await repository.GetAllInvestorsAsync());
}
