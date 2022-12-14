using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace auktionAPI.Models;

public class Auktion
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? VareId { get; set; }

    public List<Bud> BudList { get; set; } = new List<Bud>();

    public Bud CurrentBud { get; set; } = new Bud();
}
