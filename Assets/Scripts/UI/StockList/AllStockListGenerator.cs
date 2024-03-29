using System.Collections;
using System.Linq;
using UnityEngine;

public class AllStockListGenerator : StockListGenerator
{
    public override IEnumerator GenerateStockFields()
    {
        yield return base.GenerateStockFields();

        foreach (string symbol in stockHolder.AllAvailableStockSymbolsAndNames.Keys.OrderBy(x => x))
        {
            StartCoroutine(GenerateField(symbol));
        }
    }
}
