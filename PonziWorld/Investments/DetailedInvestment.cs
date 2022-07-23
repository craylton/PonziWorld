using System;

namespace PonziWorld.Investments;

internal record DetailedInvestment(
    string InvestorName,
    int InvestmentSize,
    int AmountPreviouslyInvested);
