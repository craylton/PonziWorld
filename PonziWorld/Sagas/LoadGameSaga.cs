using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Sagas;

internal class LoadGameSaga : SagaBase<LoadGameStartedEvent, LoadGameCompletedEvent>
{
    private bool hasLoadedInvestors = false;
    private bool hasLoadedDeposits = false;
    private bool hasLoadedWithdrawals = false;

    public LoadGameSaga(IEventAggregator eventAggregator)
        : base(eventAggregator)
    { }

    protected override void Start()
    {
        StartProcess(LoadInvestors.Process, new(), InvestorsLoaded);
        StartProcess(LoadCompany.Process, new(), CompanyLoaded);
    }

    private void InvestorsLoaded(InvestorsLoadedEventPayload incomingPayload)
    {
        hasLoadedInvestors = true;

        if (AreAllProcessesComplete())
            CompleteSaga();
    }

    private void CompanyLoaded(CompanyLoadedEventPayload incomingPayload) =>
        StartProcess(
            LoadInvestmentsForLastMonth.Process,
            new(incomingPayload.Company.Month),
            InvestmentsForMonthLoaded);

    private void InvestmentsForMonthLoaded(InvestmentsForLastMonthLoadedEventPayload incomingPayload)
    {
        StartProcess(
            LoadDeposits.Process,
            new(incomingPayload.LastMonthInvestments),
            DepositsLoaded);

        StartProcess(
            LoadWithdrawals.Process,
            new(incomingPayload.LastMonthInvestments),
            WithdrawalsLoaded);
    }

    private void DepositsLoaded(DepositsLoadedEventPayload incomingPayload)
    {
        hasLoadedDeposits = true;

        if (AreAllProcessesComplete())
            CompleteSaga();
    }

    private void WithdrawalsLoaded(WithdrawalsLoadedEventPayload incomingPayload)
    {
        hasLoadedWithdrawals = true;

        if (AreAllProcessesComplete())
            CompleteSaga();
    }

    private bool AreAllProcessesComplete() =>
        hasLoadedInvestors && hasLoadedDeposits && hasLoadedWithdrawals;

    protected override void ResetSaga() =>
        hasLoadedInvestors = hasLoadedDeposits = hasLoadedWithdrawals = false;
}
