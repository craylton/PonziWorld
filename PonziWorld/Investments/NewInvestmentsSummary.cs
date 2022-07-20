using PonziWorld.Investments.Investors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PonziWorld.Investments;

internal record NewInvestmentsSummary(
    IEnumerable<Investor> NewInvestors,
    IEnumerable<Investment> Reinvestments,
    IEnumerable<Investment> Withdrawals,
    IEnumerable<ProspectiveInvestor> NewProspectiveInvestors)
{
    internal NewInvestmentsSummary Sanitise()
    {
        // Those who actually invested are no longer 'prospective' investors
        IEnumerable<ProspectiveInvestor> newNewProspectiveInvestors = NewProspectiveInvestors
            .Where(prospective => !NewInvestors
                .Select(investor => investor.Id)
                .Contains(prospective.Id));

        // Combine the investments of those who had multiple investments/withdrawals in one month
        IEnumerable<Investment> combinedInvestments = GetCombinedInvestments();
        var newWithdrawals = combinedInvestments.Where(investment => investment.Amount < 0);
        var newReinvestments = combinedInvestments.Where(investment => investment.Amount > 0);

        return new NewInvestmentsSummary(
            NewInvestors.Where(investor => investor.IsActiveInvestor),
            newReinvestments,
            newWithdrawals,
            newNewProspectiveInvestors);
    }

    private IEnumerable<Investment> GetCombinedInvestments()
    {
        IEnumerable<Investment> allInvestments = Withdrawals.ToList().Concat(Reinvestments);

        return (from investment in allInvestments
                let allInvestmentsByInvestor = allInvestments.Where(inv => inv.InvestorId == investment.InvestorId)
                let totalAmountInvestedByInvestor = allInvestmentsByInvestor.Sum(inv => inv.Amount)
                select investment with { Amount = totalAmountInvestedByInvestor }).Distinct();
    }
}
