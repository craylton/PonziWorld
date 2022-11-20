using PonziWorld.Company.Processes;
using PonziWorld.DataRegion.InvestmentsTab.Processes;
using PonziWorld.DataRegion.PerformanceHistoryTab.Processes;
using PonziWorld.Events;
using PonziWorld.Investments.Investors.Processes;
using PonziWorld.MainWindow.Processes;
using Prism.Events;

namespace PonziWorld.Sagas;

internal class StartNewGameSaga : SagaBase<StartNewGameStartedEvent, StartNewGameCompletedEvent>
{
    private bool areInvestorsCleared = false;
    private bool areInvestmentsCleared = false;
    private bool isNewCompanyCreated = false;
    private bool isPerformanceCleared = false;

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
        StartProcess(ClearPerformance.Process, new(), OnPerformanceCleared);
    }

    private void OnInvestorsCleared(InvestorsClearedEventPayload incomingPayload)
    {
        areInvestorsCleared = true;

        if (IsReadyToExitMenu())
            ExitMenu();
    }

    private void OnInvestmentsCleared(InvestmentsClearedEventPayload incomingPayload)
    {
        areInvestmentsCleared = true;

        if (IsReadyToExitMenu())
            ExitMenu();
    }

    private void OnNewCompanyStarted(NewCompanyStartedEventPayload incomingPayload)
    {
        isNewCompanyCreated = true;

        if (IsReadyToExitMenu())
            ExitMenu();
    }

    private void OnPerformanceCleared(PerformanceClearedEventPayload obj)
    {
        isPerformanceCleared = true;

        if (IsReadyToExitMenu())
            ExitMenu();
    }

    private void ExitMenu() =>
        StartProcess(MainWindow.Processes.ExitMenu.Process, new(), OnMenuExited);

    private void OnMenuExited(MenuExitedEventPayload incomingPayload) => CompleteSaga();

    private bool IsReadyToExitMenu() =>
        areInvestorsCleared && areInvestmentsCleared && isNewCompanyCreated && isPerformanceCleared;

    protected override void ResetSaga() =>
        areInvestorsCleared = areInvestmentsCleared = isNewCompanyCreated = isPerformanceCleared = false;
}
