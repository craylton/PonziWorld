using Prism.Events;
using System;
using System.Threading.Tasks;

namespace PonziWorld.Core;

public static class PubSubEventExtensions
{
    public static SubscriptionToken SubscribeAsync<TPayload>(
        this PubSubEvent<TPayload> @event,
        Func<TPayload, Task> action) =>
        @event.Subscribe(payload => action(payload).Await(), true);
}
