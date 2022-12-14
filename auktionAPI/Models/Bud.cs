using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace auktionAPI.Models;

public class Bud
{
    public Guid Id { get; set; }

    public string? VareId { get; set; }

    public string? KundeId { get; set; }

    public int Value { get; set; }

    public DateTime Tidsstempel { get; set; }
}
