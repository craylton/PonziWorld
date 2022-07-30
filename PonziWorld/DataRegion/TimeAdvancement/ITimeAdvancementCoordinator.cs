using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.TimeAdvancement;

internal interface ITimeAdvancementCoordinator
{
    NewInvestmentsSummary GetNextMonthInvestments(
        Company.Company company,
        IEnumerable<Investor> investors);

    Task ApplyAsync(NewInvestmentsSummary newInvestmentsSummary);
}
