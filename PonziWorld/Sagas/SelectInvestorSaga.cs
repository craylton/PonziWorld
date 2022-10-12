using PonziWorld.DataRegion.PerformanceHistoryTab;
using PonziWorld.Events;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System;
using System.Collections.Generic;

namespace PonziWorld.Sagas;

internal class SelectInvestorSaga : SagaBase<SelectInvestorStartedEvent, SelectInvestorCompletedEvent>
{
    private bool hasSelectedInvestor = false;
    private bool hasInvestmentHistory = false;
    private bool isInvestorTabDisplayed = false;
    private bool isCorrectInvestorDisplayed = false;

    private Investor? selectedInvestor;
    private IEnumerable<MonthlyPerformance> performance = new List<MonthlyPerformance>();

    public SelectInvestorSaga(IEventAggregator eventAggregator)
        : base(eventAggregator)
    { }

    protected override void OnSagaStarted()
    {
        StartProcess(RetrieveSelectedInvestor.Process, new(), OnSelectedInvestorRetrieved);
        StartProcess(RetrieveInterestRateHistory.Process, new(), OnHistoricalInterestRatesRetrieved);
    }

    private void OnSelectedInvestorRetrieved(RetrieveSelectedInvestorEventPayload payload)
    {
        selectedInvestor = payload.Investor;
        hasSelectedInvestor = true;

        if (IsReadyToDisplayInvestorDetails())
            DisplayInvestorDetails();
    }

    private void OnHistoricalInterestRatesRetrieved(InterestRateHistoryRetrievedPayload payload)
    {
        performance = payload.Performance;
        hasInvestmentHistory = true;

        if (IsReadyToDisplayInvestorDetails())
            DisplayInvestorDetails();
    }

    private void DisplayInvestorDetails()
    {
        if (selectedInvestor is null)
            isCorrectInvestorDisplayed = true;
        else
            StartProcess(DisplayInvestor.Process, new(selectedInvestor, performance), OnInvestorDisplayed);

        StartProcess(DisplayInvestorTab.Process, new(selectedInvestor), OnInvestorTabDisplayed);
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

    private bool IsReadyToDisplayInvestorDetails() =>
        hasSelectedInvestor && hasInvestmentHistory;

    private bool IsReadyToCompleteSaga() =>
        isInvestorTabDisplayed && isCorrectInvestorDisplayed;

    protected override void ResetSaga()
    {
        hasSelectedInvestor = hasInvestmentHistory = isInvestorTabDisplayed = isCorrectInvestorDisplayed = false;
        selectedInvestor = null;
        performance = new List<MonthlyPerformance>();
    }
}
