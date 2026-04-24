namespace GildedRoseKata;

public interface IItemChangeService
{
    public (int Quality, int SellIn) CalcChange(Item item);
}
