﻿using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Events;

internal class RetrieveInvestmentsForLastMonth
    : SagaProcess<
        InvestmentsForLastMonthRetrievedEvent,
        InvestmentsForLastMonthRetrievedEventPayload,
        RetrieveInvestmentsForLastMonthCommand,
        RetrieveInvestmentsForLastMonthCommandPayload>
{
    public static RetrieveInvestmentsForLastMonth Process => new();
    private RetrieveInvestmentsForLastMonth() { }
}

internal class InvestmentsForLastMonthRetrievedEvent
    : PubSubEvent<InvestmentsForLastMonthRetrievedEventPayload>
{ }

internal record InvestmentsForLastMonthRetrievedEventPayload(
    IEnumerable<Investment> LastMonthInvestments);

internal class RetrieveInvestmentsForLastMonthCommand
    : PubSubEvent<RetrieveInvestmentsForLastMonthCommandPayload>
{ }

internal record RetrieveInvestmentsForLastMonthCommandPayload(
    int CurrentMonth);
