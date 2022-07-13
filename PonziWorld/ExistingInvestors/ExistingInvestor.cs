using MongoDB.Bson.Serialization.Attributes;

namespace PonziWorld.ExistingInvestors;

[BsonIgnoreExtraElements]
internal record ExistingInvestor(string Name, int Amount)
{
}
