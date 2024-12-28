using Core.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace Infrastructure.Data;

public class NotificationDbContext
{
    private readonly IMongoDatabase _database;

    public NotificationDbContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
        _database = client.GetDatabase(configuration.GetValue<string>("MongoDbName"));
        OnConfiguring();
    }

    public IMongoCollection<StudentNotification> StudentNotifications =>
        _database.GetCollection<StudentNotification>("StudentNotifications");

    public IMongoCollection<AdminNotification> AdminNotifications =>
        _database.GetCollection<AdminNotification>("AdminNotifications");

    private static void OnConfiguring()
    {
        BsonClassMap.RegisterClassMap<StudentNotification>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);

            cm.MapIdProperty(c => c.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetElementName("_id");

            cm.MapProperty(c => c.UserId).SetElementName("UserId");
            cm.MapProperty(c => c.Content).SetElementName("Content");
            cm.MapProperty(c => c.CreatedAt).SetElementName("CreatedAt");

            cm.MapProperty(c => c.CreatedAt)
                .SetDefaultValue(DateTime.UtcNow);
            // Ignore for now
            // cm.MapProperty(c => c.IsRead).SetElementName("IsRead");
            // cm.MapProperty(c => c.ReadAt).SetElementName("ReadAt");
        });
    }
}