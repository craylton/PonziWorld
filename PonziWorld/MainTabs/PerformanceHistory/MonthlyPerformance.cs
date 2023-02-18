using MongoDB.Bson.Serialization.Attributes;

namespace PonziWorld.MainTabs.PerformanceHistory;

[BsonIgnoreExtraElements]
internal record MonthlyPerformance(int Month, double InterestRate);