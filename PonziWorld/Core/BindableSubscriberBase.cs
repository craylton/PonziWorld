using PonziWorld.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;

namespace PonziWorld.Core;

internal abstract class BindableSubscriberBase(IEventAggregator eventAggregator) : BindableBase
{
    private readonly IEventAggregator eventAggregator = eventAggregator;

    protected void SubscribeToProcess<TCommand, TCommandPayload, TEvent, TEventPayload>(
        SagaProcess<TCommand, TCommandPayload, TEvent, TEventPayload> _,
        Func<TCommandPayload, TEventPayload> function)
        where TCommand : PubSubEvent<TCommandPayload>, new()
        where TEvent : PubSubEvent<TEventPayload>, new() =>
        eventAggregator.GetEvent<TCommand>()
            .Subscribe(
                commandPayload => GetProcessAction<TCommandPayload, TEvent, TEventPayload>(
                    function,
                    commandPayload),
                true);

    protected void SubscribeToProcess<TCommand, TCommandPayload, TEvent, TEventPayload>(
        SagaProcess<TCommand, TCommandPayload, TEvent, TEventPayload> _,
        Func<TCommandPayload, Task<TEventPayload>> function)
        where TCommand : PubSubEvent<TCommandPayload>, new()
        where TEvent : PubSubEvent<TEventPayload>, new() =>
        eventAggregator.GetEvent<TCommand>()
            .SubscribeAsync(
                commandPayload => GetProcessActionAsync<TCommandPayload, TEvent, TEventPayload>(
                    function,
                    commandPayload));

    private void GetProcessAction<TCommandPayload, TEvent, TEventPayload>(
        Func<TCommandPayload, TEventPayload> function,
        TCommandPayload payload)
        where TEvent : PubSubEvent<TEventPayload>, new() =>
        eventAggregator.GetEvent<TEvent>().Publish(function(payload));

    private async Task GetProcessActionAsync<TCommandPayload, TEvent, TEventPayload>(
        Func<TCommandPayload, Task<TEventPayload>> function,
        TCommandPayload payload)
        where TEvent : PubSubEvent<TEventPayload>, new() =>
        eventAggregator.GetEvent<TEvent>().Publish(await function(payload));
}
