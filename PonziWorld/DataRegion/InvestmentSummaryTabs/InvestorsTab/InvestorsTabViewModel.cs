using PonziWorld.Company;
using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.InvestmentSummaryTabs.InvestorsTab;

internal class InvestorsTabViewModel : BindableBase
{
    private readonly IInvestorsRepository investorsRepository;
    private readonly ICompanyRepository companyRepository;
    private readonly IInvestmentsRepository investmentsRepository;
    private ObservableCollection<DetailedInvestment> _investments = new();

    public ObservableCollection<DetailedInvestment> Investments
    {
        get => _investments;
        set => SetProperty(ref _investments, value);
    }

    public InvestorsTabViewModel(
        IInvestorsRepository investorsRepository,
        ICompanyRepository companyRepository,
        IInvestmentsRepository investmentsRepository,
        IEventAggregator eventAggregator)
    {
        this.investorsRepository = investorsRepository;
        this.companyRepository = companyRepository;

        eventAggregator.GetEvent<LoadGameRequestedEvent>()
            .Subscribe(() => LoadLastMonthInvestments().Await());

        eventAggregator.GetEvent<NextMonthRequestedEvent>()
            .Subscribe(investmentsSummary => CompileInvestmentList(investmentsSummary).Await());
        this.investmentsRepository = investmentsRepository;
    }

    private async Task LoadLastMonthInvestments()
    {
        Company.Company company = await companyRepository.GetCompanyAsync();

        IEnumerable<Investment> lastMonthInvestments = (await investmentsRepository
            .GetInvestmentsByMonthAsync(company.Month - 1))
            .Where(investment => investment.Amount > 0);

        List<DetailedInvestment> investments = await GetDetailedInvestments(lastMonthInvestments);
        SetInvestmentsList(investments);
    }

    private async Task CompileInvestmentList(NewInvestmentsSummary investmentsSummary)
    {
        List<DetailedInvestment> investments = await GetAllNewInvestments(investmentsSummary);
        SetInvestmentsList(investments);
    }

    private void SetInvestmentsList(List<DetailedInvestment> investments)
    {
        Investments.Clear();
        Investments.AddRange(investments.OrderByDescending(investment => investment.InvestmentSize));
    }

    private async Task<List<DetailedInvestment>> GetAllNewInvestments(NewInvestmentsSummary investmentsSummary)
    {
        List<DetailedInvestment> investments = investmentsSummary.NewInvestors
            .Select(investor => new DetailedInvestment(investor.Name, investor.Investment, 0))
            .ToList();

        List<DetailedInvestment> reinvestments = await GetDetailedInvestments(investmentsSummary.Reinvestments);
        investments.AddRange(reinvestments);
        return investments;
    }

    private async Task<List<DetailedInvestment>> GetDetailedInvestments(IEnumerable<Investment> investments)
    {
        List<DetailedInvestment> detailedInvestments = new();

        foreach (Investment investment in investments)
        {
            Investor investor = await investorsRepository.GetInvestorByIdAsync(investment.InvestorId);
            detailedInvestments.Add(new DetailedInvestment(
                investor.Name,
                investment.Amount,
                investor.Investment - investment.Amount));
        }

        return detailedInvestments;
    }
}
