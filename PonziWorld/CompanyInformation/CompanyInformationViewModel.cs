using Prism.Mvvm;

namespace PonziWorld.CompanyInformation;

internal class CompanyInformationViewModel : BindableBase
{
    public CompanyInformation _companyInformation = new("Company Name", 0, 0, 0, 10, 0, 5);

    public CompanyInformation CompanyInformation
    {
        get => _companyInformation;
        set => SetProperty(ref _companyInformation, value);
    }
}
