using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace auktionAPI.Services;

public class AuktionService : IAuktionService
{
    private readonly IMongoCollection<Auktion> _auktionCollection;

    public AuktionService(
        IOptions<AuktionDatabaseSettings> auktionDatabaseSettings)
    {
        var mongoClient = new MongoClient(auktionDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            auktionDatabaseSettings.Value.DatabaseName);

        _auktionCollection = mongoDatabase.GetCollection<Auktion>(
            auktionDatabaseSettings.Value.AuktionCollectionName);
    }

    public async Task<List<Auktion>> GetAsync() =>
        await _auktionCollection.Find(_ => true).ToListAsync();

    public async Task<Auktion?> GetAsync(string id) =>
        await _auktionCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Auktion newAuktion) =>
        await _auktionCollection.InsertOneAsync(newAuktion);

    public async Task UpdateAsync(string id, Auktion updatedAuktion) =>
        await _auktionCollection.ReplaceOneAsync(x => x.Id == id, updatedAuktion);

    public async Task RemoveAsync(string id) =>
        await _auktionCollection.DeleteOneAsync(x => x.Id == id);
}