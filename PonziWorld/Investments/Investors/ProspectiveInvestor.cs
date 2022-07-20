using NameGenerator;
using System;

namespace PonziWorld.Investments.Investors;

internal record ProspectiveInvestor(Guid Id, string Name, int TotalFunds)
{
    public static ProspectiveInvestor GetRandom() =>
        new(Guid.NewGuid(),
            Generator.GenerateRandomFullName(),
            Random.Shared.Next(1000));

    public virtual bool WantsToInvest(Company.Company company) =>
        Random.Shared.Next(100) < company.Attractiveness;

    public Investor AsActiveInvestor(Company.Company company)
    {
        var investmentSize = DetermineInvestmentSize(company);
        return AsInvestor(investmentSize);
    }

    public Investor AsInvestor() => new(Id, Name, TotalFunds, 0, 50);

    private Investor AsInvestor(int investmentSize) => new(Id, Name, TotalFunds, investmentSize, 50);

    public virtual int DetermineInvestmentSize(Company.Company company) =>
        Random.Shared.Next(TotalFunds * (100 - company.Suspicion) / 100);
}
