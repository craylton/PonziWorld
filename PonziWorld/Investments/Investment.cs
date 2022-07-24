using MongoDB.Bson.Serialization.Attributes;
using PonziWorld.Investments.Investors;
using System;

namespace PonziWorld.Investments;

[BsonIgnoreExtraElements]
internal record Investment(Guid Id, Guid InvestorId, int Amount, int Month)
{
    public static Investment GetNew(Investor investor, Company.Company company) =>
        new(Guid.NewGuid(),
            investor.Id,
            investor.Investment,
            company.Month);
}
