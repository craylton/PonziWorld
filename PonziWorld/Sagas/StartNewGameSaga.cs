using PonziWorld.Company;
using PonziWorld.DataRegion.InvestmentSummaryTabs.InvestmentsTab;
using PonziWorld.Events;
using PonziWorld.Investments.Investors;
using PonziWorld.MainWindow;
using Prism.Events;

namespace PonziWorld.Sagas;

internal class StartNewGameSaga : SagaBase<StartNewGameStartedEvent, StartNewGameCompletedEvent>
{
    private bool investorsCleared = false;
    private bool investmentsCleared = false;
    private bool newCompanyCreated = false;

    public StartNewGameSaga(IEventAggregator eventAggregator)
        : base(eventAggregator)
    { }

    protected override void OnSagaStarted() =>
        StartProcess(AcquireNewGameSettings.Process, new(), OnNewGameSettingsObtained);

    private void OnNewGameSettingsObtained(NewGameSettingsAcquiredEventPayload incomingPayload)
    {
        if (incomingPayload.IsCancelled)
        {
            CompleteSaga();
            return;
        }

        StartProcess(ClearInvestors.Process, new(), OnInvestorsCleared);
        StartProcess(ClearInvestments.Process, new(), OnInvestmentsCleared);
        StartProcess(StartNewCompany.Process, new(incomingPayload.CompanyName), OnNewCompanyStarted);
    }

    private void OnInvestorsCleared(InvestorsClearedEventPayload incomingPayload)
    {
        investorsCleared = true;

        if (IsReadyToExitMenu())
            ExitMenu();
    }

    private void OnInvestmentsCleared(InvestmentsClearedEventPayload incomingPayload)
    {
        investmentsCleared = true;

        if (IsReadyToExitMenu())
            ExitMenu();
    }

    private void OnNewCompanyStarted(NewCompanyStartedEventPayload incomingPayload)
    {
        newCompanyCreated = true;

        if (IsReadyToExitMenu())
            ExitMenu();
    }

    private void ExitMenu() =>
        StartProcess(MainWindow.ExitMenu.Process, new(), OnMenuExited);

    private void OnMenuExited(MenuExitedEventPayload incomingPayload) => CompleteSaga();

    private bool IsReadyToExitMenu() =>
        investorsCleared && investmentsCleared && newCompanyCreated;

    protected override void ResetSaga() =>
        investorsCleared = investmentsCleared = newCompanyCreated = false;
}
