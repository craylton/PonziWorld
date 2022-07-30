using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.MainWindow;

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
