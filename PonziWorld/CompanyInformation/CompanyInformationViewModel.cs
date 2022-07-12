using Prism.Mvvm;

namespace PonziWorld.CompanyInformation;

internal class CompanyInformationViewModel : BindableBase
{
    public int _claimedFunds = 0;
    public int _actualFunds = 0;
    public int _attractiveness = 10;
    public int _fame = 0;
    public int _suspicion = 5;

    public int ClaimedFunds
    {
        get => _claimedFunds;
        set => SetProperty(ref _claimedFunds, value);
    }

    public int ActualFunds
    {
        get => _actualFunds;
        set => SetProperty(ref _actualFunds, value);
    }

    public int Attractiveness
    {
        get => _attractiveness;
        set => SetProperty(ref _attractiveness, value);
    }

    public int Fame
    {
        get => _fame;
        set => SetProperty(ref _fame, value);
    }

    public int Suspicion
    {
        get => _suspicion;
        set => SetProperty(ref _suspicion, value);
    }
}
