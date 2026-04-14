using System;
using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose
{
    private int MaxQuality = 40;
    private int MinQuality = 0;

    IList<Item> Items;

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    public void UpdateQuality()
    {
        foreach (var item in Items)
        {
            var qualityChange = CalcQualityChange(item);
            item.Quality = Math.Clamp(item.Quality + qualityChange, MinQuality, MaxQuality);

            if (item.Name != ItemName.Sulfuras)
            {
                item.SellIn--;
            }
        }
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

    private int CalcQualityChange_Legendary(Item item)
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
            // I don't love this line - I would prefer to say "set quality to zero", rather than "subtract current quality".
            // In a more complicated system, perhaps quality could change while we are calculating, so this does not end up setting it to zero.
            return -item.Quality;
    }

    private int CalcQualityChange_Conjured(Item item)
    {
        return 2 * CalcQualityChange_Default(item);
    }
}