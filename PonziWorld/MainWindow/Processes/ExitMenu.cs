using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.MainWindow.Processes;

internal class ExitMenu
    : SagaProcess<
        ExitMenuCommand,
        ExitMenuCommandPayload,
        MenuExitedEvent,
        MenuExitedEventPayload>
{
    public static ExitMenu Process => new();
    private ExitMenu() { }
}

internal class ExitMenuCommand
    : PubSubEvent<ExitMenuCommandPayload>
{ }

internal record ExitMenuCommandPayload;

internal class MenuExitedEvent
    : PubSubEvent<MenuExitedEventPayload>
{ }

internal record MenuExitedEventPayload;
