using PonziWorld.Events;
using Prism.Events;
using Serilog;
using System;
using System.Collections.Concurrent;

namespace PonziWorld.Sagas;

internal abstract class SagaBase<TStartedEvent, TCompletedEvent>(IEventAggregator eventAggregator)
    where TStartedEvent : PubSubEvent, new()
    where TCompletedEvent : PubSubEvent, new()
{
    private readonly IEventAggregator eventAggregator = eventAggregator;
    private readonly ConcurrentDictionary<Type, SubscriptionToken> eventSubscriptions = new();
    private bool isInProgress = false;

    protected string SagaName => GetType().Name;

    public void Start()
    {
        if (isInProgress)
        {
            Log.Warning($"Could not start {SagaName} saga as it is already running");
            return;
        }

        Log.Information($"{SagaName}: Starting");
        isInProgress = true;
        eventAggregator.GetEvent<TStartedEvent>().Publish();
        OnSagaStarted();
    }

    protected abstract void OnSagaStarted();

    protected void CompleteSaga()
    {
        Log.Information($"{SagaName}: Completing");
        eventAggregator.GetEvent<TCompletedEvent>().Publish();
        ResetSaga();
        isInProgress = false;
    }

    protected virtual void ResetSaga()
    { }

    protected void StartProcess<TCommand, TCommandPayload, TEvent, TEventPayload>(
        SagaProcess<TCommand, TCommandPayload, TEvent, TEventPayload> _,
        TCommandPayload payload,
        Action<TEventPayload> onCompletion)
        where TCommand : PubSubEvent<TCommandPayload>, new()
        where TEvent : PubSubEvent<TEventPayload>, new()
    {
        Log.Debug($"{SagaName}: Sending {typeof(TCommand).Name}");

        SubscriptionToken subscriptionToken = eventAggregator.GetEvent<TEvent>()
            .Subscribe(eventPayload =>
                GetOnCompletionAction<TEvent, TEventPayload>(
                    onCompletion,
                    eventPayload),
                true);

        if (!eventSubscriptions.TryAdd(typeof(TEvent), subscriptionToken))
            Log.Warning($"{SagaName}: Already subscribed to {typeof(TEvent).Name}");

        eventAggregator.GetEvent<TCommand>().Publish(payload);
    }

    private void GetOnCompletionAction<TEvent, TEventPayload>(
        Action<TEventPayload> action,
        TEventPayload payload)
        where TEvent : PubSubEvent<TEventPayload>, new()
    {
        Log.Debug($"{SagaName}: Received {typeof(TEvent).Name}");

        eventAggregator.GetEvent<TEvent>()
            .Unsubscribe(eventSubscriptions[typeof(TEvent)]);

        if (!eventSubscriptions.TryRemove(typeof(TEvent), out _))
            Log.Warning($"{SagaName}: Something went wrong unsubscribing from {typeof(TEvent).Name}");

        action(payload);
    }
}
