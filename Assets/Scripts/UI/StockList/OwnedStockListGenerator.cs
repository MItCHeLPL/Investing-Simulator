using System.Collections;
using UnityEngine;

public class OwnedStockListGenerator : StockListGenerator
{
    public override IEnumerator GenerateStockFields()
    {
        yield return base.GenerateStockFields();

        foreach (OwnedStock ownedStock in stockHolder.OwnedStocksHolder.OwnedStocks)
        {
            StartCoroutine(GenerateField(ownedStock.Symbol));
        }
    }
}
