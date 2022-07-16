using System.Threading.Tasks;

namespace PonziWorld.Company;

internal interface ICompanyRepository
{
    Task CreateNewCompanyAsync(Company company);
    Task<Company> GetCompanyAsync();
    Task<bool> GetCompanyExistsAsync();
}
