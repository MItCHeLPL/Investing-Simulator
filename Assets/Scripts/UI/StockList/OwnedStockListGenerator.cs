using System.Collections;
using System.Linq;
using UnityEngine;

public class OwnedStockListGenerator : StockListGenerator
{
    public override IEnumerator GenerateStockFields()
    {
        yield return base.GenerateStockFields();

        foreach (OwnedStock ownedStock in stockHolder.OwnedStocksHolder.OwnedStocks.OrderBy(x => x.Symbol))
        {
            StartCoroutine(GenerateField(ownedStock.Symbol));
        }
    }
}
