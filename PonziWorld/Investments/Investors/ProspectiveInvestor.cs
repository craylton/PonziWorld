using NameGenerator;
using System;

namespace PonziWorld.Investments.Investors;

internal record ProspectiveInvestor(
    Guid Id,
    string Name,
    int TotalFunds)
    : InvestorBase(Id, Name, TotalFunds)
{
    public static ProspectiveInvestor GenerateRandom() =>
        new(Guid.NewGuid(),
            Generator.GenerateRandomFullName(),
            Random.Shared.Next(1000));

    public bool WantsToInvest(Company.Company company) =>
        Random.Shared.Next(100) < company.Attractiveness;

    public Investor AsActiveInvestor(Company.Company company)
    {
        var investmentSize = DetermineInvestmentSize(company);
        return new(Id, Name, TotalFunds, investmentSize, 50);
    }

    public Investor AsInactiveInvestor() => new(Id, Name, TotalFunds, 0, 50);

    public int DetermineInvestmentSize(Company.Company company) =>
        Random.Shared.Next(TotalFunds * (100 - company.Suspicion) / 100);
}
