namespace PonziWorld.DataRegion.InvestmentSummaryTabs.InvestorTab;

internal record HistoricalTransaction(
    int Month,
    double Amount,
    double CumulativeTotal,
    TransactionType TransactionType);
