using PonziWorld.Events;
using Prism.Events;
using System;
using System.Collections.Concurrent;

namespace PonziWorld.Sagas;

internal abstract class SagaBase<TStartedEvent, TCompletedEvent>
    where TStartedEvent : PubSubEvent, new()
    where TCompletedEvent : PubSubEvent, new()
{
    private readonly IEventAggregator eventAggregator;
    private readonly ConcurrentDictionary<Type, SubscriptionToken> eventSubscriptions = new();
    private bool isInProgress = false;

    protected SagaBase(IEventAggregator eventAggregator) =>
        this.eventAggregator = eventAggregator;

    public void Start()
    {
        if (isInProgress)
            return;

        isInProgress = true;
        eventAggregator.GetEvent<TStartedEvent>().Publish();
        StartInternal();
    }

    protected abstract void StartInternal();

    protected void CompleteSaga()
    {
        eventAggregator.GetEvent<TCompletedEvent>().Publish();
        ResetSaga();
        isInProgress = false;
    }

    protected virtual void ResetSaga()
    { }

    protected void StartProcess<TEvent, TEventPayload, TCommand, TCommandPayload>(
        SagaProcess<TEvent, TEventPayload, TCommand, TCommandPayload> _,
        TCommandPayload payload,
        Action<TEventPayload> action)
        where TEvent : PubSubEvent<TEventPayload>, new()
        where TCommand : PubSubEvent<TCommandPayload>, new()
    {
        SubscriptionToken subscriptionToken = eventAggregator.GetEvent<TEvent>().Subscribe(
            eventPayload => GetOnCompletionAction<TEvent, TEventPayload>(action, eventPayload), true);

        eventSubscriptions.TryAdd(typeof(TEvent), subscriptionToken);

        eventAggregator.GetEvent<TCommand>().Publish(payload);
    }

    private void GetOnCompletionAction<TEvent, TEventPayload>(
        Action<TEventPayload> action,
        TEventPayload payload)
        where TEvent : PubSubEvent<TEventPayload>, new()
    {
        eventAggregator.GetEvent<TEvent>()
            .Unsubscribe(eventSubscriptions[typeof(TEvent)]);

        eventSubscriptions.TryRemove(typeof(TEvent), out _);

        action(payload);
    }
}
