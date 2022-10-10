using PonziWorld.Events;
using PonziWorld.Investments.Investors;
using Prism.Events;

namespace PonziWorld.Sagas;

internal class SelectInvestorSaga : SagaBase<SelectInvestorStartedEvent, SelectInvestorCompletedEvent>
{
    public SelectInvestorSaga(IEventAggregator eventAggregator)
        : base(eventAggregator)
    { }

    protected override void OnSagaStarted() =>
        StartProcess(RetrieveSelectedInvestor.Process, new(), OnSelectedInvestorRetrieved);

    private void OnSelectedInvestorRetrieved(RetrieveSelectedInvestorEventPayload payload)
    {
        if (payload.Investor is null)
        {
            CompleteSaga();
            return;
        }

        StartProcess(DisplayInvestor.Process, new(payload.Investor), OnInvestorDisplayed);
    }

    private void OnInvestorDisplayed(InvestorDisplayedEventPayload obj) =>
        CompleteSaga();
}
