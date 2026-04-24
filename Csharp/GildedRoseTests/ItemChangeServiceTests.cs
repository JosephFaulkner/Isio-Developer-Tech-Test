using GildedRoseKata;
using Xunit;

namespace GildedRoseTests;

public class ItemChangeServiceTests
{
    [Fact]
    public void UpdateQuality_UpdatesSellInAndQuality()
    {
        // Arrange
        Item item = new Item { Name = "foo", SellIn = 10, Quality = 10 };
        ItemChangeService itemChangeService = new ItemChangeService();

        // Act
        var (newQuality, newSellIn) = itemChangeService.CalcChange(item);

        // Assert
        Assert.Equal(9, newQuality);
        Assert.Equal(9, newSellIn);
    }

    [Fact]
    public void UpdateQuality_ExpiredItemsDegradeTwiceAsFast()
    {
        // Arrange
        Item item = new Item { Name = "foo", SellIn = 0, Quality = 10 };
        ItemChangeService itemChangeService = new ItemChangeService();

        // Act
        var (newQuality, newSellIn) = itemChangeService.CalcChange(item);

        // Assert
        Assert.Equal(8, newQuality);
        Assert.Equal(-1, newSellIn);
    }

    [Theory]
    [InlineData("foo", 1)]
    [InlineData("Aged Brie", 1)]
    [InlineData("Sulfuras, Hand of Ragnaros", 1)]
    [InlineData("Backstage passes to a TAFKAL80ETC concert", 1)]
    [InlineData("Backstage passes to a TAFKAL80ETC concert", -1)]
    [InlineData("Conjured Mana Cake", 1)]
    public void UpdateQuality_QualityIsNeverNegative(string name, int initialSellIn)
    {
        // Arrange
        Item item = new Item { Name = name, SellIn = initialSellIn, Quality = 0 };
        ItemChangeService itemChangeService = new ItemChangeService();

        // Act
        var (newQuality, newSellIn) = itemChangeService.CalcChange(item);

        // Assert
        Assert.False(item.Quality < 0);
    }

    [Theory]
    [InlineData("foo", 1)]
    [InlineData("Aged Brie", 1)]
    [InlineData("Sulfuras, Hand of Ragnaros", 1)]
    [InlineData("Backstage passes to a TAFKAL80ETC concert", 1)]
    [InlineData("Backstage passes to a TAFKAL80ETC concert", -1)]
    [InlineData("Conjured Mana Cake", 1)]
    public void UpdateQuality_QualityIsNeverAbove40(string name, int initialSellIn)
    {
        // Arrange
        Item item = new Item { Name = name, SellIn = initialSellIn, Quality = 40 };
        ItemChangeService itemChangeService = new ItemChangeService();

        // Act
        var (newQuality, newSellIn) = itemChangeService.CalcChange(item);

        // Assert
        Assert.False(item.Quality > 40);
    }

    [Fact]
    public void UpdateQuality_AgedBrieIncreasesInQuality()
    {
        // Arrange
        Item item = new Item { Name = "Aged Brie", SellIn = 10, Quality = 10 };
        ItemChangeService itemChangeService = new ItemChangeService();

        // Act
        var (newQuality, newSellIn) = itemChangeService.CalcChange(item);

        // Assert
        Assert.Equal(11, newQuality);
        Assert.Equal(9, newSellIn);
    }

    [Fact]
    public void UpdateQuality_ExpiredAgedBrieIncreasesInQualityTwiceAsFast()
    {
        // Arrange
        Item item = new Item { Name = "Aged Brie", SellIn = 0, Quality = 10 };
        ItemChangeService itemChangeService = new ItemChangeService();

        // Act
        var (newQuality, newSellIn) = itemChangeService.CalcChange(item);

        // Assert
        Assert.Equal(12, newQuality);
        Assert.Equal(-1, newSellIn);
    }

    [Fact]
    public void UpdateQuality_LegendaryItemsDoNotAge()
    {
        // Arrange
        Item item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 10, Quality = 10 };
        ItemChangeService itemChangeService = new ItemChangeService();

        // Act
        var (newQuality, newSellIn) = itemChangeService.CalcChange(item);

        // Assert
        Assert.Equal(10, newQuality);
        Assert.Equal(10, newSellIn);
    }

    [Theory]
    [InlineData(8, 11)]
    [InlineData(7, 13)]
    [InlineData(3, 13)]
    [InlineData(2, 14)]
    [InlineData(1, 14)]
    [InlineData(0, 0)]
    public void UpdateQuality_BackstagePassesDependOnSellIn(int initialSellIn, int expectedQuality)
    {
        // Arrange
        Item item = new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = initialSellIn, Quality = 10 };
        ItemChangeService itemChangeService = new ItemChangeService();

        // Act
        var (newQuality, newSellIn) = itemChangeService.CalcChange(item);

        // Assert
        Assert.Equal(expectedQuality, newQuality);
        Assert.Equal(initialSellIn - 1, newSellIn);
    }

    [Theory]
    [InlineData(10, 8)]
    [InlineData(0, 6)]
    public void UpdateQuality_ConjuredItemsDegradeTwiceAsFast(int initialSellIn, int expectedQuality)
    {
        // Arrange
        Item item = new Item { Name = "Conjured Mana Cake", SellIn = initialSellIn, Quality = 10 };
        ItemChangeService itemChangeService = new ItemChangeService();

        // Act
        var (newQuality, newSellIn) = itemChangeService.CalcChange(item);

        // Assert
        Assert.Equal(expectedQuality, newQuality);
        Assert.Equal(initialSellIn - 1, newSellIn);
    }
}
