namespace auktionAPI.Models;

public class AuktionDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string AuktionCollectionName { get; set; } = null!;
}