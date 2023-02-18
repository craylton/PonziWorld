using PonziWorld.Investments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.MainTabs.TimeAdvancement;

internal interface ITimeAdvancementCoordinator
{
    NewInvestmentsSummary GetNextMonthInvestments(
        Company.Company company,
        IEnumerable<Investments.Investors.Investor> investors);

    Task ApplyAsync(NewInvestmentsSummary newInvestmentsSummary);
}
