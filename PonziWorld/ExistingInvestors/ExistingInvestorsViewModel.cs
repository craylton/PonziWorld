using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace PonziWorld.ExistingInvestors;

internal class ExistingInvestorsViewModel : BindableBase
{
    public ObservableCollection<ExistingInvestor> _existingInvestors = new();
    public ObservableCollection<ExistingInvestor> Investors
    {
        get => _existingInvestors;
        set => SetProperty(ref _existingInvestors, value);
    }

    public ExistingInvestorsViewModel()
    {
        Investors.Add(new("Alice", 1));
        Investors.Add(new("Bob", 2));
        Investors.Add(new("Christina", 3));
        Investors.Add(new("David", 4));
        Investors.Add(new("Eleanor", 5));
        Investors.Add(new("Frank", 6));
        Investors.Add(new("Gemma", 7));
        Investors.Add(new("Harry", 8));
        Investors.Add(new("India", 9));
        Investors.Add(new("Jack", 10));
        Investors.Add(new("Kerry", 11));
        Investors.Add(new("Liam", 12));
        Investors.Add(new("Melanie", 13));
        Investors.Add(new("Neil", 14));
    }
}
