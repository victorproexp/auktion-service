using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace auktionAPI.Models;

public class Vare
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ProductId { get; set; }

    [BsonElement("Titel")]
    public string Title { get; set; } = null!;

    [BsonElement("Beskrivelse")]
    public string Description { get; set; } = null!;

    public int ShowRoomId { get; set; }
    
    public double Valuation { get; set; }

    public DateTime AuktionStart { get; set; } = DateTime.Now;

    [BsonElement("Billeder")]
    public List<Uri> Images { get; set; } = new();
}
