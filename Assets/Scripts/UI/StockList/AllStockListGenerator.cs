using System.Collections;
using UnityEngine;

public class AllStockListGenerator : StockListGenerator
{
    public override IEnumerator GenerateStockFields()
    {
        yield return base.GenerateStockFields();

        foreach (string symbol in stockHolder.AllAvailableStockSymbols)
        {
            StartCoroutine(GenerateField(symbol));
        }
    }
}
