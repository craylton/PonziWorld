using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Company.Processes;

internal class LoadCompany
    : SagaProcess<
        CompanyLoadedEvent,
        CompanyLoadedEventPayload,
        LoadCompanyCommand,
        LoadCompanyCommandPayload>
{
    public static LoadCompany Process => new();
    private LoadCompany() { }
}

internal class CompanyLoadedEvent
    : PubSubEvent<CompanyLoadedEventPayload>
{ }

internal record CompanyLoadedEventPayload(
    Company Company);

internal class LoadCompanyCommand
    : PubSubEvent<LoadCompanyCommandPayload>
{ }

internal record LoadCompanyCommandPayload;
