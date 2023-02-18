using PonziWorld.Company.Processes;
using PonziWorld.Events;
using PonziWorld.Investments.Investors.Processes;
using PonziWorld.MainTabs.MonthlyInvestments.Processes;
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

    protected override void OnSagaStarted()
    {
        StartProcess(LoadInvestors.Process, new(), OnInvestorsLoaded);
        StartProcess(LoadCompany.Process, new(), OnCompanyLoaded);
    }

    private void OnInvestorsLoaded(InvestorsLoadedEventPayload incomingPayload)
    {
        hasLoadedInvestors = true;

        if (IsReadyToCompleteSaga())
            CompleteSaga();
    }

    private void OnCompanyLoaded(CompanyLoadedEventPayload incomingPayload) =>
        StartProcess(
            RetrieveInvestmentsForLastMonth.Process,
            new(incomingPayload.Company.Month),
            OnInvestmentsForMonthLoaded);

    private void OnInvestmentsForMonthLoaded(InvestmentsForLastMonthRetrievedEventPayload incomingPayload)
    {
        StartProcess(LoadDeposits.Process, new(incomingPayload.LastMonthInvestments), OnDepositsLoaded);
        StartProcess(LoadWithdrawals.Process, new(incomingPayload.LastMonthInvestments), OnWithdrawalsLoaded);
    }

    private void OnDepositsLoaded(DepositsLoadedEventPayload incomingPayload)
    {
        hasLoadedDeposits = true;

        if (IsReadyToCompleteSaga())
            CompleteSaga();
    }

    private void OnWithdrawalsLoaded(WithdrawalsLoadedEventPayload incomingPayload)
    {
        hasLoadedWithdrawals = true;

        if (IsReadyToCompleteSaga())
            CompleteSaga();
    }

    private bool IsReadyToCompleteSaga() =>
        hasLoadedInvestors && hasLoadedDeposits && hasLoadedWithdrawals;

    protected override void ResetSaga() =>
        hasLoadedInvestors = hasLoadedDeposits = hasLoadedWithdrawals = false;
}
