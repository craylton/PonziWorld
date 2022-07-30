using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Events;

internal class GenerateNewMonthInvestments
    : SagaProcess<
        NewMonthInvestmentsGeneratedEvent,
        NewMonthInvestmentsGeneratedEventPayload,
        GenerateNewMonthInvestmentsCommand,
        GenerateNewMonthInvestmentsCommandPayload>
{
    public static GenerateNewMonthInvestments Process => new();
    private GenerateNewMonthInvestments() { }
}

internal class NewMonthInvestmentsGeneratedEvent
    : PubSubEvent<NewMonthInvestmentsGeneratedEventPayload>
{ }

internal record NewMonthInvestmentsGeneratedEventPayload(
    NewInvestmentsSummary NewInvestmentsSummary);

internal class GenerateNewMonthInvestmentsCommand
    : PubSubEvent<GenerateNewMonthInvestmentsCommandPayload>
{ }

internal record GenerateNewMonthInvestmentsCommandPayload(
    Company.Company Company, IEnumerable<Investor> Investors);
