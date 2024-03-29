﻿using PonziWorld.Core;
using PonziWorld.MainTabs.Processes;
using Prism.Events;

namespace PonziWorld.MainTabs;

internal class MainTabsViewModel : BindableSubscriberBase
{
    private const string DefaultInvestorTabName = "Investor";

    private bool _isInvestorSelected = false;
    private string _selectedInvestorName = DefaultInvestorTabName;
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

    public MainTabsViewModel(
        IEventAggregator eventAggregator)
        : base(eventAggregator) =>
        SubscribeToProcess(DisplayInvestorTab.Process, ShowInvestorTab);

    private InvestorTabDisplayedEventPayload ShowInvestorTab(DisplayInvestorTabCommandPayload payload)
    {
        IsInvestorSelected = payload.Investor is not null;
        SelectedInvestorName = payload.Investor?.Name ?? DefaultInvestorTabName;
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
