namespace auktionAPI.Services;

public class BudHandler : IBudHandler
{
    public List<Auktion> AuktionList { get; set; } = new();

    public BudHandler()
    {
    }

    public Auktion? UpdateAuctionIfBidIsValid(Bud bud)
    {
        var auktion = GetAuctionIfBidIsValid(bud);
        
        if (auktion is not null)
        {
            InsertBidInAuction(bud, auktion);
        }

        return auktion;
    }

    private Auktion? GetAuctionIfBidIsValid(Bud bud)
    {
        return AuktionList.FirstOrDefault(a => IsMatchOnVare(bud, a) && IsHighestBid(bud, a));
    }

    private bool IsMatchOnVare(Bud newBud, Auktion auktion)
    {
        return auktion.VareId!.Equals(newBud.VareId);
    }

    private bool IsHighestBid(Bud newBud, Auktion auktion)
    {
        return (newBud.Value > auktion.CurrentBud.Value);
    }

    private void InsertBidInAuction(Bud newBud, Auktion auktion)
    {
        auktion.BudList.Add(newBud);
        auktion.CurrentBud = newBud;
    }
}
