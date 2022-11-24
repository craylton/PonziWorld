using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Company.Processes;

internal class LoadCompany
    : SagaProcess<
        LoadCompanyCommand,
        LoadCompanyCommandPayload,
        CompanyLoadedEvent,
        CompanyLoadedEventPayload>
{
    public static LoadCompany Process => new();
    private LoadCompany() { }
}

internal class LoadCompanyCommand
    : PubSubEvent<LoadCompanyCommandPayload>
{ }

internal record LoadCompanyCommandPayload;

internal class CompanyLoadedEvent
    : PubSubEvent<CompanyLoadedEventPayload>
{ }

internal record CompanyLoadedEventPayload(
    Company Company);
