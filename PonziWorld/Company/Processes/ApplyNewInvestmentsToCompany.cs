using PonziWorld.Events;
using PonziWorld.Investments;
using Prism.Events;

namespace PonziWorld.Company.Processes;

internal class ApplyNewInvestmentsToCompany
    : SagaProcess<
        ApplyNewInvestmentsToCompanyCommand,
        ApplyNewInvestmentsToCompanyCommandPayload,
        NewInvestmentsAppliedToCompanyEvent,
        NewInvestmentsAppliedToCompanyEventPayload>
{
    public static ApplyNewInvestmentsToCompany Process => new();
    private ApplyNewInvestmentsToCompany() { }
}

internal class ApplyNewInvestmentsToCompanyCommand
    : PubSubEvent<ApplyNewInvestmentsToCompanyCommandPayload>
{ }

internal record ApplyNewInvestmentsToCompanyCommandPayload(
    NewInvestmentsSummary NewInvestmentsSummary);

internal class NewInvestmentsAppliedToCompanyEvent
    : PubSubEvent<NewInvestmentsAppliedToCompanyEventPayload>
{ }

internal record NewInvestmentsAppliedToCompanyEventPayload(
    Company Company);
