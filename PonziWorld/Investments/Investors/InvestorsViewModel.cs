using PonziWorld.Core;
using PonziWorld.Investments.Investors.Processes;
using PonziWorld.Sagas;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PonziWorld.Investments.Investors;

internal class InvestorsViewModel : BindableSubscriberBase
{
    private readonly SelectInvestorSaga selectInvestorSaga;
    private readonly IInvestorsRepository repository;
    private ObservableCollection<Investor> _investors = new();
    private Investor? _selectedInvestor;

    public ObservableCollection<Investor> Investors
    {
        get => _investors;
        set => SetProperty(ref _investors, value);
    }

    public Investor? SelectedInvestor
    {
        get => _selectedInvestor;
        set
        {
            SetProperty(ref _selectedInvestor, value);
            selectInvestorSaga.Start();
        }
    }

    public InvestorsViewModel(
        SelectInvestorSaga selectInvestorSaga,
        IInvestorsRepository repository,
        IEventAggregator eventAggregator)
        : base(eventAggregator)
    {
        this.selectInvestorSaga = selectInvestorSaga;
        this.repository = repository;

        SubscribeToProcess(RetrieveInvestors.Process, GetAllInvestorsAsync);
        SubscribeToProcess(RetrieveSelectedInvestor.Process, GetSelectedInvestorAsync);
        SubscribeToProcess(LoadInvestors.Process, UpdateInvestorListAsync);
        SubscribeToProcess(ApplyClaimedInterestRateToInvestors.Process, OnNewInterestDeclarationAsync);
        SubscribeToProcess(ClearInvestors.Process, DeleteAllInvestorsAsync);
    }

    private async Task<InvestorsRetrievedEventPayload> GetAllInvestorsAsync(RetrieveInvestorsCommandPayload _) =>
        new(await repository.GetAllInvestorsAsync());

    private RetrieveSelectedInvestorEventPayload GetSelectedInvestorAsync(RetrieveSelectedInvestorCommandPayload _) =>
        new(SelectedInvestor);

    private async Task<InvestorsLoadedEventPayload> UpdateInvestorListAsync(LoadInvestorsCommandPayload _)
    {
        IEnumerable<Investor> investors = await repository.GetAllActiveInvestorsAsync();
        Investors.Clear();
        Investors.AddRange(investors);
        return new(investors);
    }

    private async Task<ClaimedInterestRateAppliedToInvestorsEventPayload> OnNewInterestDeclarationAsync(
        ApplyClaimedInterestRateToInvestorsCommandPayload incomingPayload)
    {
        await repository.ApplyInterestRateAsync(incomingPayload.ClaimedInterestRate);
        return new(await repository.GetAllInvestorsAsync());
    }

    private async Task<InvestorsClearedEventPayload> DeleteAllInvestorsAsync(ClearInvestorsCommandPayload _)
    {
        await repository.DeleteAllInvestorsAsync();
        return new();
    }
}
