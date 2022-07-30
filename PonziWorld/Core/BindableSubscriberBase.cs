using Prism.Events;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;

namespace PonziWorld.Core;

internal abstract class BindableSubscriberBase : BindableBase
{
    private readonly IEventAggregator eventAggregator;

    protected BindableSubscriberBase(IEventAggregator eventAggregator) =>
        this.eventAggregator = eventAggregator;

    protected void SubscribeToProcess<TEvent, TEventPayload, TCommand, TCommandPayload>(
        Events.SagaProcess<TEvent, TEventPayload, TCommand, TCommandPayload> _,
        Func<TCommandPayload, TEventPayload> function)
        where TEvent : PubSubEvent<TEventPayload>, new()
        where TCommand : PubSubEvent<TCommandPayload>, new() =>
        eventAggregator.GetEvent<TCommand>()
            .Subscribe(
                commandPayload => GetProcessAction<TEvent, TEventPayload, TCommandPayload>(
                    function,
                    commandPayload),
                true);

    protected void SubscribeToProcess<TEvent, TEventPayload, TCommand, TCommandPayload>(
        Events.SagaProcess<TEvent, TEventPayload, TCommand, TCommandPayload> _,
        Func<TCommandPayload, Task<TEventPayload>> function)
        where TEvent : PubSubEvent<TEventPayload>, new()
        where TCommand : PubSubEvent<TCommandPayload>, new() =>
        eventAggregator.GetEvent<TCommand>()
            .SubscribeAsync(
                commandPayload => GetProcessActionAsync<TEvent, TEventPayload, TCommandPayload>(
                    function,
                    commandPayload));

    private void GetProcessAction<TEvent, TEventPayload, TCommandPayload>(
        Func<TCommandPayload, TEventPayload> function,
        TCommandPayload payload)
        where TEvent : PubSubEvent<TEventPayload>, new() =>
        eventAggregator.GetEvent<TEvent>().Publish(function(payload));

    private async Task GetProcessActionAsync<TEvent, TEventPayload, TCommandPayload>(
        Func<TCommandPayload, Task<TEventPayload>> function,
        TCommandPayload payload)
        where TEvent : PubSubEvent<TEventPayload>, new() =>
        eventAggregator.GetEvent<TEvent>().Publish(await function(payload));
}
