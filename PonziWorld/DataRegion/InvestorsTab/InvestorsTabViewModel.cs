using PonziWorld.Events;
using PonziWorld.Investments;
using PonziWorld.Investments.Investors;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PonziWorld.DataRegion.InvestorsTab
{
    internal class InvestorsTabViewModel : BindableBase
    {
        private ObservableCollection<Investor> _investors = new();
        private readonly IInvestorsRepository investorRepository;

        public ObservableCollection<Investor> Investors
        {
            get => _investors;
            set => SetProperty(ref _investors, value);
        }

        public InvestorsTabViewModel(
        IInvestorsRepository investorRepository,
        IEventAggregator eventAggregator)
        {
            this.investorRepository = investorRepository;
            eventAggregator
                .GetEvent<NextMonthRequestedEvent>()
                .Subscribe(investmentsSummary => CompileInvestorList(investmentsSummary).Await());
        }

        private async Task CompileInvestorList(NewInvestmentsSummary investmentsSummary)
        {
            var investors = investmentsSummary.NewInvestors.ToList();

            foreach (var investment in investmentsSummary.Reinvestments)
            {
                var investor = await investorRepository.GetInvestorByIdAsync(investment.InvestorId);
                investors.Add(investor);
            }

            Investors.Clear();
            Investors.AddRange(investors);
        }
    }
}
