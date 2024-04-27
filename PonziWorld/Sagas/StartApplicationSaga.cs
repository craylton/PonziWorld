using PonziWorld.Events;
using PonziWorld.Splash.Processes;
using Prism.Events;

namespace PonziWorld.Sagas;

internal class StartApplicationSaga(IEventAggregator eventAggregator)
    : SagaBase<StartApplicationStartedEvent, StartApplicationCompletedEvent>(eventAggregator)
{
    protected override void OnSagaStarted() =>
        StartProcess(TestDatabaseConnection.Process, new(), OnDatabaseTestComplete);

    private void OnDatabaseTestComplete(DatabaseConnectionTestedEventPayload incomingPayload) =>
        // TODO: do something when we can't connect to the db
        CompleteSaga();
}
