using System;

namespace GildedRoseKata;

public class ItemChangeService : IItemChangeService
{
    private static int MaxQuality = 40;
    private static int MinQuality = 0;

    public (int Quality, int SellIn) CalcChange(Item item)
    {
        var qualityChange = CalcQualityChange(item);
        var newQuality = Math.Clamp(item.Quality + qualityChange, MinQuality, MaxQuality);

        var sellinChange = item.Name == ItemName.Sulfuras ? 0 : -1;
        var newSellIn = item.SellIn + sellinChange;

        return (newQuality, newSellIn);
    }

    private int CalcQualityChange(Item item)
    {
        switch (item.Name)
        {
            case ItemName.AgedBrie:
                return CalcQualityChange_AgedBrie(item);
            case ItemName.Sulfuras:
                return CalcQualityChange_Legendary(item);
            case ItemName.BackstagePass:
                return CalcQualityChange_BackstagePass(item);
            case ItemName.ConjuredItem:
                return CalcQualityChange_Conjured(item);
            default:
                return CalcQualityChange_Default(item);
        }
    }

    private int CalcQualityChange_Default(Item item)
    {
        return item.SellIn > 0 ? -1 : -2;
    }

    private int CalcQualityChange_AgedBrie(Item item)
    {
        return item.SellIn > 0 ? 1 : 2;
    }

    private  int CalcQualityChange_Legendary(Item item)
    {
        return 0;
    }

    private int CalcQualityChange_BackstagePass(Item item)
    {
        if (item.SellIn > 7)
            return 1;
        else if (item.SellIn > 2)
            return 3;
        else if (item.SellIn > 0)
            return 4;
        else
            return -item.Quality;
    }

    private int CalcQualityChange_Conjured(Item item)
    {
        return 2 * CalcQualityChange_Default(item);
    }
}
