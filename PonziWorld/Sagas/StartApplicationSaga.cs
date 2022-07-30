﻿using PonziWorld.Events;
using PonziWorld.Splash;
using Prism.Events;

namespace PonziWorld.Sagas;

internal class StartApplicationSaga : SagaBase<StartApplicationStartedEvent, StartApplicationCompletedEvent>
{
    public StartApplicationSaga(IEventAggregator eventAggregator)
        : base(eventAggregator)
    { }

    protected override void OnSagaStarted() =>
        StartProcess(TestDatabaseConnection.Process, new(), OnDatabaseTestComplete);

    private void OnDatabaseTestComplete(DatabaseConnectionTestedEventPayload incomingPayload) =>
        // TODO: do something when we can't connect to the db
        CompleteSaga();
}
