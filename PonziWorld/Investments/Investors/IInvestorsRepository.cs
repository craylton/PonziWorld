using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PonziWorld.Investments.Investors;

internal interface IInvestorsRepository
{
    Task AddInvestorAsync(Investor investor);
    Task<IEnumerable<Investor>> GetAllInvestorsAsync();
    Task<IEnumerable<Investor>> GetAllActiveInvestorsAsync();
    Task<IEnumerable<Investor>> GetAllProspectiveInvestorsAsync();
    Task<bool> GetInvestorExistsAsync(Investor newInvestor);
    Task<Investor> GetInvestorByIdAsync(Guid investorId);
    Task ApplyInvestmentAsync(Investment investment);
    Task DeleteAllInvestorsAsync();
}