using PonziWorld.Investments;
using Prism.Events;

namespace PonziWorld.Events;

internal class LoadDepositsForNewMonth
    : SagaProcess<
        DepositsForNewMonthLoadedEvent,
        DepositsForNewMonthLoadedEventPayload,
        LoadDepositsForNewMonthCommand,
        LoadDepositsForNewMonthCommandPayload>
{
    public static LoadDepositsForNewMonth Process => new();
    private LoadDepositsForNewMonth() { }
}

internal class DepositsForNewMonthLoadedEvent
    : PubSubEvent<DepositsForNewMonthLoadedEventPayload>
{ }

internal record DepositsForNewMonthLoadedEventPayload();

internal class LoadDepositsForNewMonthCommand
    : PubSubEvent<LoadDepositsForNewMonthCommandPayload>
{ }

internal record LoadDepositsForNewMonthCommandPayload(
    NewInvestmentsSummary NewInvestmentsSummary);
