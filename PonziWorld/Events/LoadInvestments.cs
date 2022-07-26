using PonziWorld.Investments;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Events;

internal class InvestmentsLoadedEvent
    : PubSubEvent
{ }

internal class LoadInvestmentsCommand
    : PubSubEvent<LoadInvestmentsCommandPayload>
{ }

internal record LoadInvestmentsCommandPayload(
    IEnumerable<Investment> Investments);
