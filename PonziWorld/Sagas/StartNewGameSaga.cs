using PonziWorld.Events;
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

    protected override void StartInternal() =>
        StartProcess(AcquireNewGameSettings.Process, new(), NewGameSettingsObtained);

    private void NewGameSettingsObtained(NewGameSettingsAcquiredEventPayload incomingPayload)
    {
        if (incomingPayload.IsCancelled)
        {
            CompleteSaga();
            return;
        }

        StartProcess(ClearInvestors.Process, new(), InvestorsCleared);
        StartProcess(ClearInvestments.Process, new(), InvestmentsCleared);
        StartProcess(StartNewCompany.Process, new(incomingPayload.CompanyName), NewCompanyStarted);
    }

    private void InvestorsCleared(InvestorsClearedEventPayload incomingPayload)
    {
        investorsCleared = true;

        if (AreAllProcessesComplete())
            ReadyToStartGame();
    }

    private void InvestmentsCleared(InvestmentsClearedEventPayload incomingPayload)
    {
        investmentsCleared = true;

        if (AreAllProcessesComplete())
            ReadyToStartGame();
    }

    private void NewCompanyStarted(NewCompanyStartedEventPayload incomingPayload)
    {
        newCompanyCreated = true;

        if (AreAllProcessesComplete())
            ReadyToStartGame();
    }

    private void ReadyToStartGame() =>
        StartProcess(ExitMenu.Process, new(), MenuExited);

    private void MenuExited(MenuExitedEventPayload incomingPayload) => CompleteSaga();

    private bool AreAllProcessesComplete() =>
        investorsCleared && investmentsCleared && newCompanyCreated;

    protected override void ResetSaga() =>
        investorsCleared = investmentsCleared = newCompanyCreated = false;
}
