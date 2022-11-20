using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Company.Processes;

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

internal record NewCompanyStartedEventPayload(
    Company Company);

internal class StartNewCompanyCommand
    : PubSubEvent<StartNewCompanyCommandPayload>
{ }

internal record StartNewCompanyCommandPayload(
    string NewCompanyName);
