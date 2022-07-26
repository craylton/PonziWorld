using Prism.Events;

namespace PonziWorld.Events;

internal class CompanyLoadedEvent
    : PubSubEvent<CompanyLoadedEventPayload>
{ }

internal record CompanyLoadedEventPayload(
    Company.Company Company);

internal class LoadCompanyCommand
    : PubSubEvent
{ }
