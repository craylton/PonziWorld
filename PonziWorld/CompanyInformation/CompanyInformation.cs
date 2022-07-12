namespace PonziWorld.CompanyInformation;

internal record CompanyInformation(
    string CompanyName,
    int Month,
    int ClaimedFunds,
    int ActualFunds,
    int Attractiveness,
    int Fame,
    int Suspicion)
{
}
