using PonziWorld.Events;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.DataRegion.PerformanceHistoryTab.Processes;

internal class RetrieveInterestRateHistory
    : SagaProcess<
        RetrieveInterestRateHistoryCommand,
        RetrieveInterestRateHistoryCommandPayload,
        InterestRateHistoryRetrieved,
        InterestRateHistoryRetrievedPayload>
{
    public static RetrieveInterestRateHistory Process => new();
    private RetrieveInterestRateHistory() { }
}

internal class RetrieveInterestRateHistoryCommand
    : PubSubEvent<RetrieveInterestRateHistoryCommandPayload>
{ }

internal record RetrieveInterestRateHistoryCommandPayload;

internal class InterestRateHistoryRetrieved
    : PubSubEvent<InterestRateHistoryRetrievedPayload>
{ }

internal record InterestRateHistoryRetrievedPayload(
    IEnumerable<MonthlyPerformance> Performance);
