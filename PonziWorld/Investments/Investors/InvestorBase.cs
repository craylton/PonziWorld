using System;

namespace PonziWorld.Investments.Investors;

internal record InvestorBase(
    Guid Id,
    string Name,
    int TotalFunds);
