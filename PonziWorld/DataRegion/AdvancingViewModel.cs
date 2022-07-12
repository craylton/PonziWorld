using Prism.Commands;
using Prism.Mvvm;
using System;

namespace PonziWorld.DataRegion;

internal class AdvancingViewModel : BindableBase
{
    public DelegateCommand NextMonthCommand { get; private set; }

    public AdvancingViewModel()
    {
        NextMonthCommand = new(GoToNextMonth, CanGoToNextMonth);
    }

    private void GoToNextMonth()
    {
        Console.WriteLine("next month");
    }

    private bool CanGoToNextMonth()
    {
        return true;
    }
}
