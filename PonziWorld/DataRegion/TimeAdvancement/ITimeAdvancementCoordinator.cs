using PonziWorld.Investments;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.TimeAdvancement;

internal interface ITimeAdvancementCoordinator
{
    Task<NewInvestmentsSummary> GetNextMonthInvestmentsAsync();
    Task ApplyAsync(NewInvestmentsSummary newInvestmentsSummary);
}
