using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.DataRegion.PerformanceHistoryTab.Processes;

internal class ClearPerformance
    : SagaProcess<
        ClearPerformanceCommand,
        ClearPerformanceCommandPayload,
        PerformanceClearedEvent,
        PerformanceClearedEventPayload>
{
    public static ClearPerformance Process => new();
    private ClearPerformance() { }
}

internal class ClearPerformanceCommand
    : PubSubEvent<ClearPerformanceCommandPayload>
{ }

internal record ClearPerformanceCommandPayload;

internal class PerformanceClearedEvent
    : PubSubEvent<PerformanceClearedEventPayload>
{ }

internal record PerformanceClearedEventPayload;
