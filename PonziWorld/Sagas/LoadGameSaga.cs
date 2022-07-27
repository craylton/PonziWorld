using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Sagas;

internal class LoadGameSaga : SagaBase<LoadGameStartedEvent, LoadGameCompletedEvent>
{
    private bool hasLoadedInvestors = false;
    private bool hasLoadedInvestments = false;
    private bool hasLoadedWithdrawals = false;

    public LoadGameSaga(IEventAggregator eventAggregator)
        : base(eventAggregator)
    { }

    protected override void Start()
    {
        StartProcess(new LoadInvestorsProcess(), new(), InvestorsLoaded);
        StartProcess(new LoadCompanyProcess(), new(), CompanyLoaded);
    }

    private void InvestorsLoaded(InvestorsLoadedEventPayload incomingPayload)
    {
        hasLoadedInvestors = true;

        if (AreAllProcessesComplete())
            CompleteSaga();
    }

    private void CompanyLoaded(CompanyLoadedEventPayload incomingPayload) =>
        StartProcess(
            new LoadInvestmentsForLastMonthProcess(),
            new(incomingPayload.Company.Month),
            InvestmentsForMonthLoaded);

    private void InvestmentsForMonthLoaded(InvestmentsForLastMonthLoadedEventPayload incomingPayload)
    {
        StartProcess(
            new LoadDepositsProcess(),
            new(incomingPayload.LastMonthInvestments),
            DepositsLoaded);

        StartProcess(
            new LoadWithdrawalsProcess(),
            new(incomingPayload.LastMonthInvestments),
            WithdrawalsLoaded);
    }

    private void DepositsLoaded(DepositsLoadedEventPayload incomingPayload)
    {
        hasLoadedInvestments = true;

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
        hasLoadedInvestors && hasLoadedInvestments && hasLoadedWithdrawals;
}
