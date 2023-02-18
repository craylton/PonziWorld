using PonziWorld.Investments;
using System;

namespace PonziWorld.MainTabs.MonthlyInvestments;

internal record DetailedInvestment(
    string InvestorName,
    double InvestmentSize,
    double AmountPreviouslyInvested)
{
    public DetailedInvestment(Investments.Investors.Investor investor, Investment investment)
        : this(
              investor.Name,
              Math.Abs(investment.Amount),
              investor.Investment - investment.Amount)
    { }
}
