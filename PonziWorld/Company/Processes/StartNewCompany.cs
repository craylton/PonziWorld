using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Company.Processes;

internal class StartNewCompany
    : SagaProcess<
        StartNewCompanyCommand,
        StartNewCompanyCommandPayload,
        NewCompanyStartedEvent,
        NewCompanyStartedEventPayload>
{
    public static StartNewCompany Process => new();
    private StartNewCompany() { }
}

internal class StartNewCompanyCommand
    : PubSubEvent<StartNewCompanyCommandPayload>
{ }

internal record StartNewCompanyCommandPayload(
    string NewCompanyName);

internal class NewCompanyStartedEvent
    : PubSubEvent<NewCompanyStartedEventPayload>
{ }

internal record NewCompanyStartedEventPayload(
    Company Company);
