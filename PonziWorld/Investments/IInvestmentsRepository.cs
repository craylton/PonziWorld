using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.Investments;

internal interface IInvestmentsRepository
{
    Task AddInvestmentAsync(Investment investment);
    Task<IEnumerable<Investment>> GetInvestmentsByMonthAsync(int month);
    Task<IEnumerable<Investment>> GetInvestmentsByInvestorIdAsync(Guid investorId);
    Task DeleteAllInvestmentsAsync();
}
