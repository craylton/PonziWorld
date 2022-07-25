using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.Investments;

internal interface IInvestmentsRepository
{
    Task AddInvestmentAsync(Investment investment);
    Task<List<Investment>> GetInvestmentsByMonthAsync(int month);
    Task DeleteAllInvestments();
}
