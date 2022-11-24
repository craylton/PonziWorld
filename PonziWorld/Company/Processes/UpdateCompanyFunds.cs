using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;

namespace PonziWorld.Company.Processes;

internal class UpdateCompanyFunds
    : SagaProcess<
        UpdateCompanyFundsCommand,
        UpdateCompanyFundsCommandPayload,
        CompanyFundsUpdatedEvent,
        CompanyFundsUpdatedEventPayload>
{
    public static UpdateCompanyFunds Process => new();
    private UpdateCompanyFunds() { }
}

internal class UpdateCompanyFundsCommand
    : PubSubEvent<UpdateCompanyFundsCommandPayload>
{ }

internal record UpdateCompanyFundsCommandPayload(
    NewInvestmentsSummary NewInvestmentsSummary);

internal class CompanyFundsUpdatedEvent
    : PubSubEvent<CompanyFundsUpdatedEventPayload>
{ }

internal record CompanyFundsUpdatedEventPayload(
    Company Company);
