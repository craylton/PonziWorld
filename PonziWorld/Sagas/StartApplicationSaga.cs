using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Sagas;

internal class StartApplicationSaga : SagaBase<StartApplicationStartedEvent, StartApplicationCompletedEvent>
{
    public StartApplicationSaga(IEventAggregator eventAggregator)
        : base(eventAggregator)
    { }

    protected override void Start() =>
        StartProcess(TestDatabaseConnection.Process, new(), DatabaseTestComplete);

    private void DatabaseTestComplete(DatabaseConnectionTestedEventPayload incomingPayload) =>
        // TODO: do something when we can't connect to the db
        CompleteSaga();
}
