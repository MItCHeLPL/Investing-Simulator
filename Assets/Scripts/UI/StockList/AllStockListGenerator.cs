using System.Collections;
using UnityEngine;

public class AllStockListGenerator : StockListGenerator
{
    public override IEnumerator GenerateStockFields()
    {
        yield return base.GenerateStockFields();

        foreach (Stock stock in stockHolder.AllStocks)
        {
            GenerateField(stock);
        }
    }
}
