using PonziWorld.Events;
using PonziWorld.Investments.Investors;
using Prism.Events;

namespace PonziWorld.Sagas;

internal class SelectInvestorSaga : SagaBase<SelectInvestorStartedEvent, SelectInvestorCompletedEvent>
{
    private bool isInvestorTabDisplayed = false;
    private bool isCorrectInvestorDisplayed = false;

    public SelectInvestorSaga(IEventAggregator eventAggregator)
        : base(eventAggregator)
    { }

    protected override void OnSagaStarted() =>
        StartProcess(RetrieveSelectedInvestor.Process, new(), OnSelectedInvestorRetrieved);

    private void OnSelectedInvestorRetrieved(RetrieveSelectedInvestorEventPayload payload)
    {
        if (payload.Investor is null)
            isCorrectInvestorDisplayed = true;
        else
            StartProcess(DisplayInvestor.Process, new(payload.Investor), OnInvestorDisplayed);

        StartProcess(DisplayInvestorTab.Process, new(payload.Investor), OnInvestorTabDisplayed);

    }

    private void OnInvestorDisplayed(InvestorDisplayedEventPayload payload)
    {
        isCorrectInvestorDisplayed = true;

        if (IsReadyToCompleteSaga())
            CompleteSaga();
    }

    private void OnInvestorTabDisplayed(InvestorTabDisplayedEventPayload payload)
    {
        isInvestorTabDisplayed = true;

        if (IsReadyToCompleteSaga())
            CompleteSaga();
    }

    private bool IsReadyToCompleteSaga() =>
        isInvestorTabDisplayed && isCorrectInvestorDisplayed;

    protected override void ResetSaga() =>
        isInvestorTabDisplayed = isCorrectInvestorDisplayed = false;
}
