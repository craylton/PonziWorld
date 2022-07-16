using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.Investors;

internal interface IInvestorsRepository
{
    Task AddInvestorAsync(Investor investor);
    Task<IEnumerable<Investor>> GetAllInvestorsAsync();
    Task DeleteAllInvestors();
}