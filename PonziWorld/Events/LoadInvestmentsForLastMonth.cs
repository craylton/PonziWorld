using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Events;

internal class LoadInvestmentsForLastMonth
    : SagaProcess<
        InvestmentsForLastMonthLoadedEvent,
        InvestmentsForLastMonthLoadedEventPayload,
        LoadInvestmentsForLastMonthCommand,
        LoadInvestmentsForLastMonthCommandPayload>
{
    public static LoadInvestmentsForLastMonth Process => new();
    private LoadInvestmentsForLastMonth() { }
}

internal class InvestmentsForLastMonthLoadedEvent
    : PubSubEvent<InvestmentsForLastMonthLoadedEventPayload>
{ }

internal record InvestmentsForLastMonthLoadedEventPayload(
    IEnumerable<Investment> LastMonthInvestments);

internal class LoadInvestmentsForLastMonthCommand
    : PubSubEvent<LoadInvestmentsForLastMonthCommandPayload>
{ }

internal record LoadInvestmentsForLastMonthCommandPayload(
    int Month);
