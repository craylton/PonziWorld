using PonziWorld.Events;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Investments.Investors;

internal class ApplyNewInterestRate
    : SagaProcess<
        NewInterestRateAppliedEvent,
        NewInterestRateAppliedEventPayload,
        ApplyNewInterestRateCommand,
        ApplyNewInterestRateCommandPayload>
{
    public static ApplyNewInterestRate Process => new();
    private ApplyNewInterestRate() { }
}

internal class ApplyNewInterestRateCommand
    : PubSubEvent<ApplyNewInterestRateCommandPayload>
{ }

internal record ApplyNewInterestRateCommandPayload(
    double ClaimedInterest);

internal class NewInterestRateAppliedEvent
    : PubSubEvent<NewInterestRateAppliedEventPayload>
{ }

internal record NewInterestRateAppliedEventPayload(
    IEnumerable<Investor> AllInvestors);
