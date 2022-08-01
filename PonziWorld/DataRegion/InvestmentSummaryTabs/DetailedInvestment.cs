using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using System;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs;

internal record DetailedInvestment(
    string InvestorName,
    double InvestmentSize,
    double AmountPreviouslyInvested)
{
    public DetailedInvestment(Investor investor, Investment investment)
        : this(
              investor.Name,
              Math.Abs(investment.Amount),
              investor.Investment - investment.Amount)
    { }
}
