﻿using MongoDB.Bson.Serialization.Attributes;

namespace PonziWorld.Company;

[BsonIgnoreExtraElements]
internal record Company(
    string Name,
    int Month,
    double ClaimedFunds,
    double ActualFunds,
    int Attractiveness,
    int Fame,
    int Suspicion)
{
    public static Company Default =>
        new("Not Loaded", 0, 0, 0, 0, 0, 0);
}
