using NameGenerator;
using System;

namespace PonziWorld.Investments.Investors;

internal record ProspectiveInvestor(
    Guid Id,
    string Name,
    double TotalFunds)
    : InvestorBase(Id, Name, TotalFunds)
{
    public static ProspectiveInvestor GenerateRandom() =>
        new(Guid.NewGuid(),
            Generator.GenerateRandomFullName(),
            Random.Shared.Next(1000));

    public override bool WantsToInvest(Company.Company company) =>
        Random.Shared.Next(100) < company.Attractiveness;

    public Investor AsActiveInvestor(Company.Company company)
    {
        double investmentSize = DetermineInvestmentSize(company);
        return new(Id, Name, TotalFunds, investmentSize, 50);
    }

    public Investor AsInactiveInvestor() => new(Id, Name, TotalFunds, 0, 50);

    private double DetermineInvestmentSize(Company.Company company) =>
        Random.Shared.NextDouble() * TotalFunds * (100 - company.Suspicion) / 100d;
}
