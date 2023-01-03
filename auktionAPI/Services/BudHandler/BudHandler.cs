namespace auktionAPI.Services;

public class BudHandler : IBudHandler
{
    private Bud bud = new();
    
    public List<Auktion> AuktionList { get; set; } = new();

    public Auktion? UpdateAuctionIfBidIsValid(Bud newBud)
    {
        bud = newBud;

        var auktion = GetAuctionIfBidIsValid();
        
        if (auktion is not null)
        {
            InsertBidInAuction(auktion);
        }

        return auktion;
    }

    private Auktion? GetAuctionIfBidIsValid()
    {
        return AuktionList.FirstOrDefault(a => IsMatchOnVare(a) && IsHighestBid(a));
    }

    private bool IsMatchOnVare(Auktion auktion)
    {
        return auktion.VareId!.Equals(bud.VareId);
    }

    private bool IsHighestBid(Auktion auktion)
    {
        return (bud.Value > auktion.CurrentBud.Value);
    }

    private void InsertBidInAuction(Auktion auktion)
    {
        auktion.BudList.Add(bud);
        auktion.CurrentBud = bud;
    }
}
