using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;

namespace PonziWorld.Company;

internal class UpdateCompanyFunds
    : SagaProcess<
        CompanyFundsUpdatedEvent,
        CompanyFundsUpdatedEventPayload,
        UpdateCompanyFundsCommand,
        UpdateCompanyFundsCommandPayload>
{
    public static UpdateCompanyFunds Process => new();
    private UpdateCompanyFunds() { }
}

internal class CompanyFundsUpdatedEvent
    : PubSubEvent<CompanyFundsUpdatedEventPayload>
{ }

internal record CompanyFundsUpdatedEventPayload(
    Company Company);

internal class UpdateCompanyFundsCommand
    : PubSubEvent<UpdateCompanyFundsCommandPayload>
{ }

internal record UpdateCompanyFundsCommandPayload(
    NewInvestmentsSummary NewInvestmentsSummary);
