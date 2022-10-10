using PonziWorld.Core;
using PonziWorld.Investments.Investors;
using Prism.Events;

namespace PonziWorld.DataRegion;

internal class DataRegionViewModel : BindableSubscriberBase
{
    private const string DefaulInvestorTabName = "Investor";

    private bool _isInvestorSelected = false;
    private string _selectedInvestorName = DefaulInvestorTabName;
    private int _selectedTabIndex = 1;

    public bool IsInvestorSelected
    {
        get => _isInvestorSelected;
        set => SetProperty(ref _isInvestorSelected, value);
    }

    public string SelectedInvestorName
    {
        get => _selectedInvestorName;
        set => SetProperty(ref _selectedInvestorName, value);
    }

    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => SetProperty(ref _selectedTabIndex, value);
    }

    public DataRegionViewModel(
        IEventAggregator eventAggregator)
        : base(eventAggregator) =>
        SubscribeToProcess(DisplayInvestorTab.Process, ShowInvestorTab);

    private InvestorDisplayedEventPayload ShowInvestorTab(DisplayInvestorTabCommandPayload payload)
    {
        IsInvestorSelected = payload.Investor is not null;
        SelectedInvestorName = payload.Investor?.Name ?? DefaulInvestorTabName;
        OpenTabAfterInvestorSelect();
        return new();
    }

    private void OpenTabAfterInvestorSelect()
    {
        if (IsInvestorSelected)
            SelectedTabIndex = 0;
        else if (SelectedTabIndex == 0)
            SelectedTabIndex = -1;
    }
}
