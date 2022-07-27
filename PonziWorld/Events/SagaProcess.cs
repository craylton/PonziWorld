using Prism.Events;

namespace PonziWorld.Events;

internal abstract class SagaProcess<TEvent, TEventPayload, TCommand, TCommandPayload>
    where TEvent : PubSubEvent<TEventPayload>, new()
    where TCommand : PubSubEvent<TCommandPayload>, new()
{ }
