using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.ExistingInvestors;

internal interface IInvestorsRepository
{
    Task AddInvestorAsync(ExistingInvestor investor);
    Task<IEnumerable<ExistingInvestor>> GetAllInvestorsAsync();
}