using Xunit;
using System.Collections.Generic;
using GildedRoseKata;

namespace GildedRoseTests;

public class GildedRoseTests
{
    [Fact]
    public void UpdateQuality_UpdatesSellInAndQuality()
    {
        // Arrange
        IList<Item> Items = new List<Item> { new Item { Name = "foo", SellIn = 10, Quality = 10 } };
        GildedRose app = new GildedRose(Items);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal("foo", Items[0].Name);
        Assert.Equal(9, Items[0].SellIn);
        Assert.Equal(9, Items[0].Quality);
    }

    [Fact]
    public void UpdateQuality_ExpiredItemsDegradeTwiceAsFast()
    {
        // Arrange
        IList<Item> Items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 10 } };
        GildedRose app = new GildedRose(Items);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal("foo", Items[0].Name);
        Assert.Equal(-1, Items[0].SellIn);
        Assert.Equal(8, Items[0].Quality);
    }

    [Fact]
    public void UpdateQuality_QualityIsNeverNegative()
    {
        // Arrange
        IList<Item> Items = new List<Item> { 
            new Item { Name = "foo", SellIn = 1, Quality = 0 },
            new Item { Name = "bar", SellIn = 1, Quality = 0 },
            new Item { Name = "Aged Brie", SellIn = 1, Quality = 0 },
            new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 1, Quality = 0 },
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 1, Quality = 0 },
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = -1, Quality = 0 },
            new Item { Name = "Conjured Mana Cake", SellIn = 1, Quality = 0 }
        };
        GildedRose app = new GildedRose(Items);

        // Act
        app.UpdateQuality();

        // Assert
        foreach (var item in Items)
        {
            Assert.False(item.Quality < 0);
        }
    }


    [Fact]
    public void UpdateQuality_QualityIsNeverAbove40()
    {
        // Arrange
        IList<Item> Items = new List<Item> {
            new Item { Name = "foo", SellIn = 1, Quality = 40 },
            new Item { Name = "bar", SellIn = 1, Quality = 40 },
            new Item { Name = "Aged Brie", SellIn = 1, Quality = 40 },
            new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 1, Quality = 40 },
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 1, Quality = 40 },
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = -1, Quality = 40 },
            new Item { Name = "Conjured Mana Cake", SellIn = 1, Quality = 40 }
        };
        GildedRose app = new GildedRose(Items);

        // Act
        app.UpdateQuality();

        // Assert
        foreach (var item in Items)
        {
            Assert.False(item.Quality > 40);
        }
    }

    [Fact]
    public void UpdateQuality_AgedBrieIncreasesInQuality()
    {
        // Arrange
        IList<Item> Items = new List<Item> { 
            new Item { Name = "Aged Brie", SellIn = 10, Quality = 10 },
            new Item { Name = "Aged Brie", SellIn = 0, Quality = 10 }
        };
        GildedRose app = new GildedRose(Items);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal("Aged Brie", Items[0].Name);
        Assert.Equal(9, Items[0].SellIn);
        Assert.Equal(11, Items[0].Quality);

        Assert.Equal("Aged Brie", Items[1].Name);
        Assert.Equal(-1, Items[1].SellIn);
        Assert.Equal(12, Items[1].Quality);
    }

    [Fact]
    public void UpdateQuality_LegendaryItemsDoNotAge()
    {
        // Arrange
        IList<Item> Items = new List<Item> { new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 10, Quality = 10 } };
        GildedRose app = new GildedRose(Items);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal("Sulfuras, Hand of Ragnaros", Items[0].Name);
        Assert.Equal(10, Items[0].SellIn);
        Assert.Equal(10, Items[0].Quality);
    }

    [Fact]
    public void UpdateQuality_BackstagePassesDependOnSellIn()
    {
        // Arrange
        IList<Item> Items = new List<Item> {
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 8, Quality = 10 },
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 7, Quality = 10 },
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 3, Quality = 10 },
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 2, Quality = 10 },
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 1, Quality = 10 },
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 0, Quality = 10 }
        };
        GildedRose app = new GildedRose(Items);

        // Act
        app.UpdateQuality();

        // Assert
        foreach (var item in Items)
        {
            Assert.Equal("Backstage passes to a TAFKAL80ETC concert", item.Name);
        }

        Assert.Equal(7, Items[0].SellIn);
        Assert.Equal(11, Items[0].Quality);

        Assert.Equal(6, Items[1].SellIn);
        Assert.Equal(13, Items[1].Quality);

        Assert.Equal(2, Items[2].SellIn);
        Assert.Equal(13, Items[2].Quality);

        Assert.Equal(1, Items[3].SellIn);
        Assert.Equal(14, Items[3].Quality);

        Assert.Equal(0, Items[4].SellIn);
        Assert.Equal(14, Items[4].Quality);

        Assert.Equal(-1, Items[5].SellIn);
        Assert.Equal(0, Items[5].Quality);
    }

    [Fact]
    public void UpdateQuality_ConjuredItemsDegradeTwiceAsFast()
    {
        // Arrange
        IList<Item> Items = new List<Item> {
            new Item { Name = "Conjured Mana Cake", SellIn = 10, Quality = 10 },
            new Item { Name = "Conjured Mana Cake", SellIn = 0, Quality = 10 }
        };
        GildedRose app = new GildedRose(Items);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal("Conjured Mana Cake", Items[0].Name);
        Assert.Equal(9, Items[0].SellIn);
        Assert.Equal(8, Items[0].Quality);

        Assert.Equal("Conjured Mana Cake", Items[1].Name);
        Assert.Equal(-1, Items[1].SellIn);
        Assert.Equal(6, Items[1].Quality);
    }
}