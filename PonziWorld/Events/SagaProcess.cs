using Prism.Events;

namespace PonziWorld.Events;

internal class SagaProcess<TEvent, TEventPayload, TCommand, TCommandPayload>
    where TEvent : PubSubEvent<TEventPayload>
    where TCommand : PubSubEvent<TCommandPayload>
{ }
