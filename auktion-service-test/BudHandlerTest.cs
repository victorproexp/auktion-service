namespace auktion_service_test;

public class BudHandlerTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void MakeValidBid()
    {
        // Arrange
        Bud newBud = CreateBidWithValueOnItem(200, "vare1");
        BudHandler budHandler = CreateBudHandlerWithAuctionWithItem("vare1");

        // Act
        budHandler.MakeBid(newBud);

        // Assert
        Bud actual = budHandler.AuktionList[0].BudList[0];
        Assert.That(newBud.Value, Is.EqualTo(actual.Value));
    }

    [Test]
    public void Make2ValidBids()
    {
        // Arrange
        Bud newBud = CreateBidWithValueOnItem(200, "vare1");
        Bud newBud2 = CreateBidWithValueOnItem(250, "vare1");
        BudHandler budHandler = CreateBudHandlerWithAuctionWithItem("vare1");

        // Act
        budHandler.MakeBid(newBud);
        budHandler.MakeBid(newBud2);

        // Assert
        List<Bud> expectedList = new List<Bud>() { 
            newBud,
            newBud2
        };
        List<Bud> actualList = budHandler.AuktionList[0].BudList;

        Bud expectedCurrentBud = newBud2;
        Bud actualCurrentBud = budHandler.AuktionList[0].CurrentBud;

        Assert.That(expectedList, Is.EqualTo(actualList));
        Assert.That(expectedCurrentBud, Is.EqualTo(actualCurrentBud));
    }

    [Test]
    public void IgnoreInvalidBid()
    {
        // Arrange
        Bud newBud = CreateBidWithValueOnItem(200, "vare1");
        Bud invalidBud = CreateBidWithValueOnItem(150, "vare1");
        BudHandler budHandler = CreateBudHandlerWithAuctionWithItem("vare1");

        // Act
        budHandler.MakeBid(newBud);
        budHandler.MakeBid(invalidBud);

        // Assert
        List<Bud> expectedList = new List<Bud>() { 
            newBud
        };
        List<Bud> actualList = budHandler.AuktionList[0].BudList;

        int expectedValue = 200;
        int actualValue = budHandler.AuktionList[0].BudList[0].Value;

        Assert.That(expectedList, Is.EqualTo(actualList));
        Assert.That(expectedValue, Is.EqualTo(actualValue));
    }

    [Test]
    public void Make2ValidBidsOn2Auctions()
    {
        // Arrange
        Bud newBud200Vare1 = CreateBidWithValueOnItem(200, "vare1");
        Bud newBud300Vare2 = CreateBidWithValueOnItem(300, "vare2");
        Bud newBud250Vare1 = CreateBidWithValueOnItem(250, "vare1");
        Bud newBud350Vare2 = CreateBidWithValueOnItem(350, "vare2");
        BudHandler budHandler = CreateBudHandlerWithAuctionWith2Items("vare1", "vare2");

        // Act
        budHandler.MakeBid(newBud200Vare1);
        budHandler.MakeBid(newBud300Vare2);
        budHandler.MakeBid(newBud250Vare1);
        budHandler.MakeBid(newBud350Vare2);

        // Assert
        List<Bud> expectedList1 = new List<Bud>() { 
            newBud200Vare1,
            newBud250Vare1
        };
        List<Bud> expectedList2 = new List<Bud>() { 
            newBud300Vare2,
            newBud350Vare2
        };
        List<Bud> actualList1 = budHandler.AuktionList[0].BudList;
        List<Bud> actualList2 = budHandler.AuktionList[1].BudList;

        Assert.That(expectedList1, Is.EqualTo(actualList1));
        Assert.That(expectedList2, Is.EqualTo(actualList2));
    }

    private Bud CreateBidWithValueOnItem(int value, string vareId)
    {
        Bud bud = new Bud() {
            Id = "bud1", 
            VareId = vareId, 
            KundeId = "kunde1", 
            Value = value, 
            Tidsstempel = DateTime.Now
        };

        return bud;
    }

    private BudHandler CreateBudHandlerWithAuctionWithItem(string vareId)
    {
        BudHandler budHandler = new BudHandler() { 
            AuktionList = new List<Auktion>() { 
                new Auktion() { 
                    Id = "auktion1", 
                    VareId = vareId
                }
            }
        };

        return budHandler;
    }
    
    private BudHandler CreateBudHandlerWithAuctionWith2Items(string vareId, string vareId2)
    {
        BudHandler budHandler = new BudHandler() { 
            AuktionList = new List<Auktion>() { 
                new Auktion() { 
                    Id = "auktion1", 
                    VareId = vareId
                },
                new Auktion() { 
                    Id = "auktion1", 
                    VareId = vareId2
                }
            }
        };

        return budHandler;
    }
}
