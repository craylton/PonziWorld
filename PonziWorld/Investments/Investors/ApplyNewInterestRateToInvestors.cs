using PonziWorld.Events;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Investments.Investors;

internal class ApplyNewInterestRateToInvestors
    : SagaProcess<
        NewInterestRateAppliedToInvestorsEvent,
        NewInterestRateAppliedToInvestorsEventPayload,
        ApplyNewInterestRateToInvestorsCommand,
        ApplyNewInterestRateToInvestorsCommandPayload>
{
    public static ApplyNewInterestRateToInvestors Process => new();
    private ApplyNewInterestRateToInvestors() { }
}

internal class ApplyNewInterestRateToInvestorsCommand
    : PubSubEvent<ApplyNewInterestRateToInvestorsCommandPayload>
{ }

internal record ApplyNewInterestRateToInvestorsCommandPayload(
    double ClaimedInterestRate);

internal class NewInterestRateAppliedToInvestorsEvent
    : PubSubEvent<NewInterestRateAppliedToInvestorsEventPayload>
{ }

internal record NewInterestRateAppliedToInvestorsEventPayload(
    IEnumerable<Investor> AllInvestors);
