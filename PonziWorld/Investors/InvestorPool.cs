using NameGenerator;
using PonziWorld.Company;
using System;
using System.Threading.Tasks;

namespace PonziWorld.Investors;

internal class InvestorPool : IInvestorPool
{
    private readonly IInvestorsRepository investorsRepository;
    private readonly ICompanyRepository companyRepository;
    private readonly Random random = new();

    public InvestorPool(
        IInvestorsRepository investorsRepository,
        ICompanyRepository companyRepository)
    {
        this.investorsRepository = investorsRepository;
        this.companyRepository = companyRepository;
    }

    public async Task AddInvestorsToPool()
    {
        var company = await companyRepository.GetCompanyAsync();

        double numberOfNewInvestors = random.Next((int)Math.Ceiling((Math.Pow(company.Fame, 3) / 1100) + company.Fame));

        for (int i = 0; i < numberOfNewInvestors; i++)
        {
            var newInvestor = new Investor(
                        Generator.GenerateRandomFullName(),
                        random.Next(1000),
                        0,
                        random.Next(40, 60));

            if (random.Next(100) < company.Attractiveness)
            {
                var investmentSize = random.Next(newInvestor.TotalFunds * (100 - company.Suspicion) / 100);
                newInvestor = newInvestor with { Investment = investmentSize };
            }

            await investorsRepository.AddInvestorAsync(newInvestor);
        }
    }
}
