using PonziWorld.Investments;
using Prism.Events;

namespace PonziWorld.Events;

internal class NewMonthInvestmentsGeneratedEvent : PubSubEvent<NewInvestmentsSummary>
{
}