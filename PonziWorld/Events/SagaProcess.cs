using Prism.Events;

namespace PonziWorld.Events;

internal abstract class SagaProcess<TCommand, TCommandPayload, TEvent, TEventPayload>
    where TCommand : PubSubEvent<TCommandPayload>, new()
    where TEvent : PubSubEvent<TEventPayload>, new()
{ }
