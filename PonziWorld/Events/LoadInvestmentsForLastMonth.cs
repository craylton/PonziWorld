using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Events;

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
