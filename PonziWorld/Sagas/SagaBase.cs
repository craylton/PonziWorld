using PonziWorld.Events;
using Prism.Events;
using Serilog;
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

    protected string SagaName => GetType().Name;

    protected SagaBase(IEventAggregator eventAggregator) =>
        this.eventAggregator = eventAggregator;

    public void Start()
    {
        if (isInProgress)
        {
            Log.Logger.Warning($"Could not start {SagaName} saga as it is already running");
            return;
        }

        Log.Logger.Information($"{SagaName} saga starting");
        isInProgress = true;
        eventAggregator.GetEvent<TStartedEvent>().Publish();
        OnSagaStarted();
    }

    protected abstract void OnSagaStarted();

    protected void CompleteSaga()
    {
        Log.Logger.Information($"{SagaName} saga completing");
        eventAggregator.GetEvent<TCompletedEvent>().Publish();
        ResetSaga();
        isInProgress = false;
    }

    protected virtual void ResetSaga()
    { }

    protected void StartProcess<TEvent, TEventPayload, TCommand, TCommandPayload>(
        SagaProcess<TEvent, TEventPayload, TCommand, TCommandPayload> _,
        TCommandPayload payload,
        Action<TEventPayload> onCompletion)
        where TEvent : PubSubEvent<TEventPayload>, new()
        where TCommand : PubSubEvent<TCommandPayload>, new()
    {
        Log.Logger.Information($"{SagaName} saga sending {typeof(TCommand).Name} command");

        SubscriptionToken subscriptionToken = eventAggregator.GetEvent<TEvent>()
            .Subscribe(eventPayload =>
                GetOnCompletionAction<TEvent, TEventPayload>(
                    onCompletion,
                    eventPayload),
                true);

        eventSubscriptions.TryAdd(typeof(TEvent), subscriptionToken);

        eventAggregator.GetEvent<TCommand>().Publish(payload);
    }

    private void GetOnCompletionAction<TEvent, TEventPayload>(
        Action<TEventPayload> action,
        TEventPayload payload)
        where TEvent : PubSubEvent<TEventPayload>, new()
    {
        Log.Logger.Information($"{SagaName} saga received {typeof(TEvent).Name} event");

        eventAggregator.GetEvent<TEvent>()
            .Unsubscribe(eventSubscriptions[typeof(TEvent)]);

        eventSubscriptions.TryRemove(typeof(TEvent), out _);

        action(payload);
    }
}
