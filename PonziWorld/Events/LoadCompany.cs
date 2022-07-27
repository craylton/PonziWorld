using Prism.Events;

namespace PonziWorld.Events;

internal class LoadCompanyProcess
    : SagaProcess<
        CompanyLoadedEvent,
        CompanyLoadedEventPayload,
        LoadCompanyCommand,
        LoadCompanyCommandPayload>
{ }

internal class CompanyLoadedEvent
    : PubSubEvent<CompanyLoadedEventPayload>
{ }

internal record CompanyLoadedEventPayload(
    Company.Company Company);

internal class LoadCompanyCommand
    : PubSubEvent<LoadCompanyCommandPayload>
{ }

internal record LoadCompanyCommandPayload;
