using MongoDB.Bson.Serialization.Attributes;
using System;

namespace PonziWorld.Investments.Investors;

[BsonIgnoreExtraElements]
internal record Investor(
    Guid Id,
    string Name,
    int TotalFunds,
    int Investment,
    int Satisfaction)
    : ProspectiveInvestor(Id, Name, TotalFunds)
{
    public bool IsActiveInvestor => Investment > 0;

    internal static Investor FromProspective(ProspectiveInvestor investor, int investmentSize) =>
        new(investor.Id, investor.Name, investor.TotalFunds, investmentSize, 50);

    public override bool WantsToInvest(Company.Company company) =>
        IsActiveInvestor
            ? ExistingInvestorWantsToReinvest()
            : ProspectiveInvestorWantsToReinvest(company);

    private bool ExistingInvestorWantsToReinvest() =>
        Random.Shared.Next(400) < Satisfaction;

    private static bool ProspectiveInvestorWantsToReinvest(Company.Company company) =>
        Random.Shared.Next(400) < company.Attractiveness;

    public Investment DetermineInvestment(Company.Company company)
    {
        var investmentSize = DetermineInvestmentSize(company);
        return new Investment(Id, investmentSize);
    }

    public override int DetermineInvestmentSize(Company.Company company)
    {
        var availableFunds = TotalFunds - Investment;
        double multiplier = (100 - company.Suspicion) / (double)100;
        return Random.Shared.Next((int)(availableFunds * multiplier));
    }

    internal bool WantsToWithdraw() =>
        Random.Shared.Next(100) < Math.Pow(100 - Satisfaction, 2) / 100;

    internal Investment DetermineWithdrawal(Company.Company company)
    {
        var investmentSize = DetermineWithdrawalSize(company);
        return new Investment(Id, -investmentSize);
    }

    internal int DetermineWithdrawalSize(Company.Company company)
    {
        double multiplier = company.Suspicion / (double)100;
        return Random.Shared.Next((int)(Investment * multiplier));
    }
}
