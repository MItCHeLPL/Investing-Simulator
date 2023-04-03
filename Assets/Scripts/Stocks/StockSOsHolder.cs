using System.Collections.Generic;
using UnityEngine;

public class StockSOsHolder : ScriptableObject
{
    public List<Stock> AllSavedStocks = new List<Stock>();


    //Get Stock from list if symbol exists
    public bool TryGetStock(string symbol, out Stock stock)
    {
        stock = null;

        for (int j = 0; j < AllSavedStocks.Count; j++)
        {
            Stock s = AllSavedStocks[j];

            if (s.Symbol == symbol)
            {
                stock = s;

                return true;
            }
        }

        return false;
    }


    [ContextMenu("Clear")]
    public void Clear()
    {
        AllSavedStocks.Clear();
    }
}
