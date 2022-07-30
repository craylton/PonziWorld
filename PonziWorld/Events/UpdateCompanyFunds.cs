using PonziWorld.Investments;
using Prism.Events;

namespace PonziWorld.Events;

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

internal record CompanyFundsUpdatedEventPayload();

internal class UpdateCompanyFundsCommand
    : PubSubEvent<UpdateCompanyFundsCommandPayload>
{ }

internal record UpdateCompanyFundsCommandPayload(
    NewInvestmentsSummary NewInvestmentsSummary);
