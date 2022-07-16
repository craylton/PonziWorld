using MongoDB.Bson.Serialization.Attributes;

namespace PonziWorld.Investors;

[BsonIgnoreExtraElements]
internal record Investor(
    string Name,
    int TotalFunds,
    int Investment,
    int Satisfaction)
{
}
