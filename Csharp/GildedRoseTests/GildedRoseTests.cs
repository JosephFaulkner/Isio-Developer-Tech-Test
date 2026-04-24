using Xunit;
using System.Collections.Generic;
using GildedRoseKata;
using Moq;

namespace GildedRoseTests;

public class GildedRoseTests
{
    [Fact]
    public void UpdateQuality_UpdatesSellInAndQuality()
    {
        // Arrange
        int expectedQuality = 8;
        int expectedSellIn = 12;
        Mock<IItemChangeService> service = new Mock<IItemChangeService>();
        service.Setup(m => m.CalcChange(It.IsAny<Item>())).Returns((expectedQuality, expectedSellIn));
        
        IList<Item> Items = new List<Item> { new Item { Name = "foo", SellIn = 10, Quality = 10 } };
        GildedRose app = new GildedRose(Items, service.Object);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal("foo", Items[0].Name);
        Assert.Equal(expectedQuality, Items[0].Quality);
        Assert.Equal(expectedSellIn, Items[0].SellIn);
    }
}