namespace auktionAPI.Services;

public interface IBudHandler
{
    List<Auktion> AuktionList { get; set; }
    Auktion? MakeBid(Bud bud);
}