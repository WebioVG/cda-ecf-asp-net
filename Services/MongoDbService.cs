using cda_ecf_asp_net.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using cda_ecf_asp_net.Repositories;

public class MongoDbService
{
    private readonly IMongoCollection<BsonDocument> _statsCollection;
    private readonly IEventRepository _eventRepository;

    public MongoDbService(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoDbSettings, IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;

        var database = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _statsCollection = database.GetCollection<BsonDocument>(mongoDbSettings.Value.StatsCollectionName);
    }

    // Populate MongoDB with stats calculated from SQL database
    public async Task PopulateStatsAsync()
    {
        // Calculate stats using IEventRepository
        var totalEvents = await _eventRepository.GetTotalEventsAsync();
        var upcomingEvents = await _eventRepository.GetUpcomingEventsAsync();
        var pastEvents = await _eventRepository.GetPastEventsAsync();
        var averageDuration = await _eventRepository.GetAverageEventDurationAsync();
        var averageParticipants = await _eventRepository.GetAverageParticipantsPerEventAsync();
        var mostPopularEvent = await _eventRepository.GetMostPopularEventAsync();

        // Prepare a stats document
        var statsDocument = new BsonDocument
        {
            { "GeneratedAt", DateTime.UtcNow },
            { "TotalEvents", totalEvents },
            { "UpcomingEvents", upcomingEvents },
            { "PastEvents", pastEvents },
            { "AverageDurationInHours", averageDuration },
            { "AverageParticipantsPerEvent", averageParticipants },
            { "MostPopularEvent", mostPopularEvent ?? "N/A" }
        };

        // Insert stats into MongoDB
        await _statsCollection.InsertOneAsync(statsDocument);
    }

    // Retrieve the latest stats document from MongoDB
    public async Task<StatsViewModel?> GetLatestStatsAsync()
    {
        var latestStats = await _statsCollection
            .Find(Builders<BsonDocument>.Filter.Empty)
            .SortByDescending(doc => doc["GeneratedAt"])
            .FirstOrDefaultAsync();

        if (latestStats == null)
        {
            return null;
        }

        return new StatsViewModel
        {
            TotalEvents = latestStats["TotalEvents"].AsInt64,
            UpcomingEvents = latestStats["UpcomingEvents"].AsInt64,
            PastEvents = latestStats["PastEvents"].AsInt64,
            AverageDurationInHours = latestStats["AverageDurationInHours"].AsDouble,
            AverageParticipantsPerEvent = latestStats["AverageParticipantsPerEvent"].AsDouble,
            MostPopularEvent = latestStats["MostPopularEvent"].AsString
        };
    }
}
