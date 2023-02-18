namespace PonziWorld.MainTabs.Investor;

internal record HistoricalTransaction(
    int Month,
    double Amount,
    double CumulativeTotal,
    TransactionType TransactionType);
