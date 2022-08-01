using MongoDB.Bson.Serialization.Attributes;
using System;

namespace PonziWorld.Investments.Investors;

[BsonIgnoreExtraElements]
internal record Investor(
    Guid Id,
    string Name,
    double TotalFunds,
    double Investment,
    int Satisfaction)
    : InvestorBase(Id, Name, TotalFunds)
{
    public bool IsActiveInvestor => Investment > 0;

    internal static Investor FromProspective(ProspectiveInvestor investor, int investmentSize) =>
        new(investor.Id, investor.Name, investor.TotalFunds, investmentSize, 50);

    public override bool WantsToInvest(Company.Company company) =>
        IsActiveInvestor
            ? ExistingInvestorWantsToReinvest()
            : ProspectiveInvestorWantsToInvest(company);

    private bool ExistingInvestorWantsToReinvest() =>
        Random.Shared.Next(400) < Satisfaction;

    private static bool ProspectiveInvestorWantsToInvest(Company.Company company) =>
        Random.Shared.Next(400) < company.Attractiveness;

    public Investment DetermineInvestment(Company.Company company)
    {
        double investmentSize = DetermineInvestmentSize(company);
        return new Investment(Guid.NewGuid(), Id, investmentSize, company.Month);
    }

    public double DetermineInvestmentSize(Company.Company company)
    {
        double availableFunds = TotalFunds - Investment;
        double multiplier = (100 - company.Suspicion) / 100d;
        return Random.Shared.NextDouble() * availableFunds * multiplier;
    }

    public bool WantsToWithdraw() =>
        Random.Shared.Next(100) < Math.Pow(100 - Satisfaction, 2) / 100;

    public Investment DetermineWithdrawal(Company.Company company)
    {
        int investmentSize = DetermineWithdrawalSize(company);
        return new Investment(Guid.NewGuid(), Id, -investmentSize, company.Month);
    }

    private int DetermineWithdrawalSize(Company.Company company)
    {
        double multiplier = company.Suspicion / 100d;
        return Random.Shared.Next((int)(Investment * multiplier));
    }
}
