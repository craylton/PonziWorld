using Prism.Events;

namespace PonziWorld.Events;

internal class StartNewCompany
    : SagaProcess<
        NewCompanyStartedEvent,
        NewCompanyStartedEventPayload,
        StartNewCompanyCommand,
        StartNewCompanyCommandPayload>
{
    public static StartNewCompany Process => new();
    private StartNewCompany() { }
}

internal class NewCompanyStartedEvent
    : PubSubEvent<NewCompanyStartedEventPayload>
{ }

internal record NewCompanyStartedEventPayload;

internal class StartNewCompanyCommand
    : PubSubEvent<StartNewCompanyCommandPayload>
{ }

internal record StartNewCompanyCommandPayload(
    string NewName);
