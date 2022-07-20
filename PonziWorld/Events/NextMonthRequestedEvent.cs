using PonziWorld.Investments;
using Prism.Events;

namespace PonziWorld.Events;

internal class NextMonthRequestedEvent : PubSubEvent<NewInvestmentsSummary>
{
}