using MongoDB.Bson.Serialization.Attributes;

namespace PonziWorld.DataRegion.PerformanceHistoryTab;

[BsonIgnoreExtraElements]
internal record MonthlyPerformance(int Month, double InterestRate);