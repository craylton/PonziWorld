using Prism.Events;

namespace PonziWorld.Events;

internal abstract class SagaProcess<TEvent, TEventPayload, TCommand, TCommandPayload>
    where TEvent : PubSubEvent<TEventPayload>
    where TCommand : PubSubEvent<TCommandPayload>
{ }
