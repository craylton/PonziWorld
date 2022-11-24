using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.MainWindow.Processes;

internal class AcquireNewGameSettings
    : SagaProcess<
        AcquireNewGameSettingsCommand,
        AcquireNewGameSettingsCommandPayload,
        NewGameSettingsAcquiredEvent,
        NewGameSettingsAcquiredEventPayload>
{
    public static AcquireNewGameSettings Process => new();
    private AcquireNewGameSettings() { }
}

internal class AcquireNewGameSettingsCommand
    : PubSubEvent<AcquireNewGameSettingsCommandPayload>
{ }

internal record AcquireNewGameSettingsCommandPayload;

internal class NewGameSettingsAcquiredEvent
    : PubSubEvent<NewGameSettingsAcquiredEventPayload>
{ }

internal record NewGameSettingsAcquiredEventPayload(
    string CompanyName,
    bool IsCancelled);
