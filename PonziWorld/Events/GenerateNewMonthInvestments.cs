using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.Events;

internal class GenerateNewMonthInvestments
    : SagaProcess<
        NewMonthInvestmentsGeneratedEvent2,
        NewMonthInvestmentsGeneratedEventPayload,
        GenerateNewMonthInvestmentsCommand,
        GenerateNewMonthInvestmentsCommandPayload>
{
    public static GenerateNewMonthInvestments Process => new();
    private GenerateNewMonthInvestments() { }
}

internal class NewMonthInvestmentsGeneratedEvent2
    : PubSubEvent<NewMonthInvestmentsGeneratedEventPayload>
{ }

internal record NewMonthInvestmentsGeneratedEventPayload(
    NewInvestmentsSummary NewInvestmentsSummary);

internal class GenerateNewMonthInvestmentsCommand
    : PubSubEvent<GenerateNewMonthInvestmentsCommandPayload>
{ }

internal record GenerateNewMonthInvestmentsCommandPayload(
    Company.Company Company, IEnumerable<Investor> Investors);
