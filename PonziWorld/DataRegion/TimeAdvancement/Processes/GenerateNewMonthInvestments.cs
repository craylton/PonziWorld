using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using System.Collections.Generic;

namespace PonziWorld.DataRegion.TimeAdvancement.Processes;

internal class GenerateNewMonthInvestments
    : SagaProcess<
        GenerateNewMonthInvestmentsCommand,
        GenerateNewMonthInvestmentsCommandPayload,
        NewMonthInvestmentsGeneratedEvent,
        NewMonthInvestmentsGeneratedEventPayload>
{
    public static GenerateNewMonthInvestments Process => new();
    private GenerateNewMonthInvestments() { }
}

internal class GenerateNewMonthInvestmentsCommand
    : PubSubEvent<GenerateNewMonthInvestmentsCommandPayload>
{ }

internal record GenerateNewMonthInvestmentsCommandPayload(
    Company.Company Company,
    IEnumerable<Investor> Investors);

internal class NewMonthInvestmentsGeneratedEvent
    : PubSubEvent<NewMonthInvestmentsGeneratedEventPayload>
{ }

internal record NewMonthInvestmentsGeneratedEventPayload(
    NewInvestmentsSummary NewInvestmentsSummary);
