using System.Collections;
using UnityEngine;

public class OwnedStockListGenerator : StockListGenerator
{
    public override IEnumerator GenerateStockFields()
    {
        yield return base.GenerateStockFields();

        foreach (string symbol in stockHolder.StockListHolder.OwnedStockSymbols)
        {
            StartCoroutine(GenerateField(symbol));
        }
    }
}
