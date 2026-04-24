using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose
{
    private IItemChangeService itemChangeService;

    IList<Item> Items;

    public GildedRose(IList<Item> Items, IItemChangeService itemChangeService)
    {
        this.Items = Items;
        this.itemChangeService = itemChangeService;
    }

    public void UpdateQuality()
    {
        foreach (var item in Items)
        {
            var (newQuality, newSellIn) = itemChangeService.CalcChange(item);
            item.Quality = newQuality;
            item.SellIn = newSellIn;
        }
    }
}