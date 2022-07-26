using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Sagas;

internal class LoadGameSaga
{
    private readonly IEventAggregator eventAggregator;
    private bool hasLoadedInvestors = false;
    private bool hasLoadedInvestments = false;
    private bool hasLoadedWithdrawals = false;

    public LoadGameSaga(IEventAggregator eventAggregator)
    {
        this.eventAggregator = eventAggregator;
    }

    public void Start()
    {
        eventAggregator.GetEvent<InvestorsLoadedEvent>().Subscribe(InvestorsLoaded);
        eventAggregator.GetEvent<LoadInvestorsCommand>().Publish();

        eventAggregator.GetEvent<CompanyLoadedEvent>().Subscribe(CompanyLoaded);
        eventAggregator.GetEvent<LoadCompanyCommand>().Publish();
    }

    private void InvestorsLoaded()
    {
        eventAggregator.GetEvent<InvestorsLoadedEvent>().Unsubscribe(InvestorsLoaded);
        hasLoadedInvestors = true;

        if (HasLoaded())
            CompleteSaga();
    }

    private void CompanyLoaded(CompanyLoadedEventPayload incomingPayload)
    {
        eventAggregator.GetEvent<CompanyLoadedEvent>().Unsubscribe(CompanyLoaded);
        LoadInvestmentsForLastMonthCommandPayload payload = new(incomingPayload.Company.Month);

        eventAggregator.GetEvent<InvestmentsForLastMonthLoadedEvent>().Subscribe(InvestmentsForMonthLoaded);
        eventAggregator.GetEvent<LoadInvestmentsForLastMonthCommand>().Publish(payload);
    }

    private void InvestmentsForMonthLoaded(InvestmentsForLastMonthLoadedEventPayload incomingPayload)
    {
        eventAggregator.GetEvent<InvestmentsForLastMonthLoadedEvent>().Unsubscribe(InvestmentsForMonthLoaded);
        LoadInvestmentsCommandPayload investmentsPayload = new(incomingPayload.LastMonthInvestments);
        LoadWithdrawalsCommandPayload withdrawalsPayload = new(incomingPayload.LastMonthInvestments);

        eventAggregator.GetEvent<InvestmentsLoadedEvent>().Subscribe(InvestmentsLoaded);
        eventAggregator.GetEvent<LoadInvestmentsCommand>().Publish(investmentsPayload);

        eventAggregator.GetEvent<WithdrawalsLoadedEvent>().Subscribe(WithdrawalsLoaded);
        eventAggregator.GetEvent<LoadWithdrawalsCommand>().Publish(withdrawalsPayload);
    }

    private void InvestmentsLoaded()
    {
        eventAggregator.GetEvent<InvestmentsLoadedEvent>().Unsubscribe(InvestmentsLoaded);
        hasLoadedInvestments = true;

        if (HasLoaded())
            CompleteSaga();
    }

    private void WithdrawalsLoaded()
    {
        eventAggregator.GetEvent<WithdrawalsLoadedEvent>().Unsubscribe(WithdrawalsLoaded);
        hasLoadedWithdrawals = true;

        if (HasLoaded())
            CompleteSaga();
    }

    private void CompleteSaga() =>
        eventAggregator.GetEvent<LoadGameCompletedEvent>().Publish();

    private bool HasLoaded() =>
        hasLoadedInvestors && hasLoadedInvestments && hasLoadedWithdrawals;
}
