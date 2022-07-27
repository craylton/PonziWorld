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

    public void SubscribeToProcess<TEvent, TEventPayload, TCommand, TCommandPayload>(
        Events.SagaProcess<TEvent, TEventPayload, TCommand, TCommandPayload> _,
        Func<TCommandPayload, Task<TEventPayload>> function)
        where TEvent : PubSubEvent<TEventPayload>, new()
        where TCommand : PubSubEvent<TCommandPayload>, new() =>
        eventAggregator.GetEvent<TCommand>()
            .SubscribeAsync(commandPayload => GetProcessFunction<TEvent, TEventPayload, TCommandPayload>(function, commandPayload));

    private async Task GetProcessFunction<TEvent, TEventPayload, TCommandPayload>(
        Func<TCommandPayload, Task<TEventPayload>> function,
        TCommandPayload payload)
        where TEvent : PubSubEvent<TEventPayload>, new()
    {
        TEventPayload eventPayload = await function(payload);
        eventAggregator.GetEvent<TEvent>().Publish(eventPayload);
    }
}
