using Prism.Events;

namespace PonziWorld.Events;

internal class ExitMenu
    : SagaProcess<
        MenuExitedEvent,
        MenuExitedEventPayload,
        ExitMenuCommand,
        ExitMenuCommandPayload>
{
    public static ExitMenu Process => new();
    private ExitMenu() { }
}

internal class MenuExitedEvent
    : PubSubEvent<MenuExitedEventPayload>
{ }

internal record MenuExitedEventPayload;

internal class ExitMenuCommand
    : PubSubEvent<ExitMenuCommandPayload>
{ }

internal record ExitMenuCommandPayload;
