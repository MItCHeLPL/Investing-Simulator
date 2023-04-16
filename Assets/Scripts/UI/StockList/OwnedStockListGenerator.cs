using System.Collections;
using UnityEngine;

public class OwnedStockListGenerator : StockListGenerator
{
    public override IEnumerator GenerateStockFields()
    {
        yield return base.GenerateStockFields();

        foreach (string symbol in stockHolder.StockListHolder.OwnedStockSymbols)
        {
            GenerateField(stockHolder.StockListHolder.AllSavedStocks[stockHolder.StockListHolder.AllSavedStocks.FindIndex(x => x.Symbol == symbol)]);
        }
    }
}
