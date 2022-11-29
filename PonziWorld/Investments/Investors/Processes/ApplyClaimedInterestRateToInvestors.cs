using PonziWorld.Events;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Investments.Investors;

internal class ApplyClaimedInterestRateToInvestors
    : SagaProcess<
        ApplyClaimedInterestRateToInvestorsCommand,
        ApplyClaimedInterestRateToInvestorsCommandPayload,
        ClaimedInterestRateAppliedToInvestorsEvent,
        ClaimedInterestRateAppliedToInvestorsEventPayload>
{
    public static ApplyClaimedInterestRateToInvestors Process => new();
    private ApplyClaimedInterestRateToInvestors() { }
}

internal class ApplyClaimedInterestRateToInvestorsCommand
    : PubSubEvent<ApplyClaimedInterestRateToInvestorsCommandPayload>
{ }

internal record ApplyClaimedInterestRateToInvestorsCommandPayload(
    double ClaimedInterestRate);

internal class ClaimedInterestRateAppliedToInvestorsEvent
    : PubSubEvent<ClaimedInterestRateAppliedToInvestorsEventPayload>
{ }

internal record ClaimedInterestRateAppliedToInvestorsEventPayload(
    IEnumerable<Investor> UpdatedInvestors);
