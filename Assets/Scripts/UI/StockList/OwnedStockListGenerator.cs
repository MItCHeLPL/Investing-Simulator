using System.Collections;
using UnityEngine;

public class OwnedStockListGenerator : StockListGenerator
{
    public override IEnumerator GenerateStockFields()
    {
        yield return base.GenerateStockFields();

        foreach (int stockId in stockHolder.OwnedStocksFromAll)
        {
            GenerateField(stockHolder.AllStocks[stockId]);
        }
    }
}
