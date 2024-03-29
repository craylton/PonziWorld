﻿using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Investments.Investors.Processes;

internal class ClearInvestors
    : SagaProcess<
        ClearInvestorsCommand,
        ClearInvestorsCommandPayload,
        InvestorsClearedEvent,
        InvestorsClearedEventPayload>
{
    public static ClearInvestors Process => new();
    private ClearInvestors() { }
}

internal class ClearInvestorsCommand
    : PubSubEvent<ClearInvestorsCommandPayload>
{ }

internal record ClearInvestorsCommandPayload;

internal class InvestorsClearedEvent
    : PubSubEvent<InvestorsClearedEventPayload>
{ }

internal record InvestorsClearedEventPayload;
