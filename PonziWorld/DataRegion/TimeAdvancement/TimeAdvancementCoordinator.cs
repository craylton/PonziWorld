using PonziWorld.Company;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.TimeAdvancement;

internal class TimeAdvancementCoordinator : ITimeAdvancementCoordinator
{
    private readonly IInvestorsRepository investorsRepository;
    private readonly ICompanyRepository companyRepository;

    public TimeAdvancementCoordinator(
        IInvestorsRepository investorsRepository,
        ICompanyRepository companyRepository)
    {
        this.investorsRepository = investorsRepository;
        this.companyRepository = companyRepository;
    }

    public async Task<NewInvestmentsSummary> GetNextMonthInvestmentsAsync()
    {
        Company.Company company = await companyRepository.GetCompanyAsync();
        IEnumerable<Investor> existingInvestors = await investorsRepository.GetAllActiveInvestorsAsync();

        // 1. Generate a list of people who have just heard about the company and _might_ invest
        List<ProspectiveInvestor> prospectiveInvestors = GenerateNewProspectiveInvestors(company.Fame).ToList();

        // 2. Out of those new people, get a list of those who actually will invest
        List<Investor> newInvestors = GetNewInvestors(prospectiveInvestors, company).ToList();

        // 2.1. People who didn't invest in previous months can still invest
        newInvestors.AddRange(await GetNewInvestorsFromPool(company));

        // 3. Existing investors might want to invest even more
        List<Investment> reinvestments = GetReinvestments(existingInvestors, company).ToList();

        // 4. Existing investors might want to withdraw some funds
        List<Investment> withdrawals = GetWithdrawals(existingInvestors, company).ToList();

        var investmentsSummary = new NewInvestmentsSummary(
            newInvestors,
            reinvestments,
            withdrawals,
            prospectiveInvestors);

        return investmentsSummary.Sanitise();
    }

    public async Task ApplyAsync(NewInvestmentsSummary newInvestmentsSummary)
    {
        foreach (Investor newInvestor in newInvestmentsSummary.NewInvestors)
        {
            if (await investorsRepository.GetInvestorExistsAsync(newInvestor))
            {
                var investment = new Investment(newInvestor.Id, newInvestor.Investment);
                await investorsRepository.ApplyInvestmentAsync(investment);
            }
            else
            {
                await investorsRepository.AddInvestorAsync(newInvestor);
            }
        }

        foreach (Investment investment in newInvestmentsSummary.Reinvestments)
        {
            await investorsRepository.ApplyInvestmentAsync(investment);
        }

        foreach (Investment withdrawal in newInvestmentsSummary.Withdrawals)
        {
            await investorsRepository.ApplyInvestmentAsync(withdrawal);
        }

        foreach (ProspectiveInvestor investor in newInvestmentsSummary.NewProspectiveInvestors)
        {
            await investorsRepository.AddInvestorAsync(investor.AsInactiveInvestor());
        }
    }

    private static IEnumerable<ProspectiveInvestor> GenerateNewProspectiveInvestors(int companyFame)
    {
        int numberOfNewInvestors = GetNumberOfNewInvestors(companyFame);
        return Enumerable.Range(0, numberOfNewInvestors)
            .Select(_ => ProspectiveInvestor.GenerateRandom());
    }

    private static IEnumerable<Investor> GetNewInvestors(
        IEnumerable<ProspectiveInvestor> prospectiveInvestors,
        Company.Company company) =>
        prospectiveInvestors
            .Where(prospectiveInvestor => prospectiveInvestor.WantsToInvest(company))
            .Select(prospective => prospective.AsActiveInvestor(company));

    private async Task<IEnumerable<Investor>> GetNewInvestorsFromPool(Company.Company company)
    {
        IEnumerable<Investor> prospectiveInvestors = await investorsRepository.GetAllProspectiveInvestorsAsync();

        return prospectiveInvestors
            .Where(prospectiveInvestor => prospectiveInvestor.WantsToInvest(company))
            .Select(prospective => prospective with
            {
                Investment = prospective.DetermineInvestmentSize(company)
            });
    }

    private static IEnumerable<Investment> GetReinvestments(
        IEnumerable<Investor> existingInvestors,
        Company.Company company) =>
        existingInvestors
            .Where(investor => investor.WantsToInvest(company))
            .Select(prospective => prospective.DetermineInvestment(company));

    private static IEnumerable<Investment> GetWithdrawals(
        IEnumerable<Investor> existingInvestors,
        Company.Company company) =>
        existingInvestors
            .Where(investor => investor.WantsToWithdraw())
            .Select(withdrawer => withdrawer.DetermineWithdrawal(company));

    private static int GetNumberOfNewInvestors(int companyFame) =>
        Random.Shared.Next((int)Math.Ceiling(Math.Pow(companyFame, 3) / 1100 + companyFame));
}
