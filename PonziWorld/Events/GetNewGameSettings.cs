using Prism.Events;

namespace PonziWorld.Events;

internal class GetNewGameSettings
    : SagaProcess<
        NewGameSettingsObtainedEvent,
        NewGameSettingsObtainedEventPayload,
        GetNewGameSettingsCommand,
        GetNewGameSettingsCommandPayload>
{
    public static GetNewGameSettings Process => new();
    private GetNewGameSettings() { }
}

internal class NewGameSettingsObtainedEvent
    : PubSubEvent<NewGameSettingsObtainedEventPayload>
{ }

internal record NewGameSettingsObtainedEventPayload(
    string CompanyName, bool IsCancelled);

internal class GetNewGameSettingsCommand
    : PubSubEvent<GetNewGameSettingsCommandPayload>
{ }

internal record GetNewGameSettingsCommandPayload;
