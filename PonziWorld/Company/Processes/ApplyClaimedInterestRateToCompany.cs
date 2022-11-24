using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Company.Processes;

internal class ApplyClaimedInterestRateToCompany
    : SagaProcess<
        ApplyClaimedInterestRateToCompanyCommand,
        ApplyClaimedInterestRateToCompanyCommandPayload,
        ClaimedInterestRateAppliedToCompanyEvent,
        ClaimedInterestRateAppliedToCompanyEventPayload>
{
    public static ApplyClaimedInterestRateToCompany Process => new();
    private ApplyClaimedInterestRateToCompany() { }
}

internal class ApplyClaimedInterestRateToCompanyCommand
    : PubSubEvent<ApplyClaimedInterestRateToCompanyCommandPayload>
{ }

internal record ApplyClaimedInterestRateToCompanyCommandPayload(
    double ClaimedInterestRate);

internal class ClaimedInterestRateAppliedToCompanyEvent
    : PubSubEvent<ClaimedInterestRateAppliedToCompanyEventPayload>
{ }

internal record ClaimedInterestRateAppliedToCompanyEventPayload(
    Company Company);
