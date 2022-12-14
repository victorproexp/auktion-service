namespace auktionAPI.Services;

public interface IAuktionService
{
    Task<List<Auktion>> GetAsync();
    Task<Auktion?> GetAsync(string id);
    Task CreateAsync(Auktion newAuktion);
    Task UpdateAsync(string id, Auktion updatedAuktion);
    Task RemoveAsync(string id);
}