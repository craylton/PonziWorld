using PonziWorld.Events;
using Prism.Events;
using System;

namespace PonziWorld.Sagas;

internal abstract class SagaBase<TStartedEvent, TCompletedEvent>
        where TStartedEvent : PubSubEvent, new()
        where TCompletedEvent : PubSubEvent, new()
{
    private readonly IEventAggregator eventAggregator;

    protected SagaBase(IEventAggregator eventAggregator) =>
        this.eventAggregator = eventAggregator;

    public void StartSaga()
    {
        eventAggregator.GetEvent<TStartedEvent>().Publish();
        Start();
    }

    protected abstract void Start();

    protected void CompleteSaga() =>
        eventAggregator.GetEvent<TCompletedEvent>().Publish();

    protected void StartProcess<TEvent, TEventPayload, TCommand, TCommandPayload>(
        SagaProcess<TEvent, TEventPayload, TCommand, TCommandPayload> _,
        TCommandPayload payload,
        Action<TEventPayload> action)
        where TEvent : PubSubEvent<TEventPayload>, new()
        where TCommand : PubSubEvent<TCommandPayload>, new()
    {
        eventAggregator.GetEvent<TEvent>().Subscribe(
            eventPayload => GetOnCompletionAction<TEvent, TEventPayload>(action, eventPayload));

        eventAggregator.GetEvent<TCommand>().Publish(payload);
    }

    private void GetOnCompletionAction<TEvent, TEventPayload>(
        Action<TEventPayload> action,
        TEventPayload payload)
        where TEvent : PubSubEvent<TEventPayload>, new()
    {
        eventAggregator.GetEvent<TEvent>().Unsubscribe(
            payload => GetOnCompletionAction<TEvent, TEventPayload>(payload => action(payload), payload));

        action(payload);
    }
}
