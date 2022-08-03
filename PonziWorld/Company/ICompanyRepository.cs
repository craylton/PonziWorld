using System.Threading.Tasks;

namespace PonziWorld.Company;

internal interface ICompanyRepository
{
    Task CreateNewCompanyAsync(Company company);
    Task<Company> GetCompanyAsync();
    Task<bool> GetCompanyExistsAsync();
    Task MoveToNextMonthAsync(double totalChangeInFunds);
    Task AddProfitAsync(double profit);
    Task ClaimInterest(double claimedInterestRate);
}
