using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.MainWindow;

internal class AcquireNewGameSettings
    : SagaProcess<
        NewGameSettingsAcquiredEvent,
        NewGameSettingsAcquiredEventPayload,
        AcquireNewGameSettingsCommand,
        AcquireNewGameSettingsCommandPayload>
{
    public static AcquireNewGameSettings Process => new();
    private AcquireNewGameSettings() { }
}

internal class NewGameSettingsAcquiredEvent
    : PubSubEvent<NewGameSettingsAcquiredEventPayload>
{ }

internal record NewGameSettingsAcquiredEventPayload(
    string CompanyName,
    bool IsCancelled);

internal class AcquireNewGameSettingsCommand
    : PubSubEvent<AcquireNewGameSettingsCommandPayload>
{ }

internal record AcquireNewGameSettingsCommandPayload;
