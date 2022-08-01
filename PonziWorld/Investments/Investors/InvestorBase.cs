using System;

namespace PonziWorld.Investments.Investors;

internal abstract record InvestorBase(
    Guid Id,
    string Name,
    double TotalFunds)
{
    public abstract bool WantsToInvest(Company.Company company);
}
