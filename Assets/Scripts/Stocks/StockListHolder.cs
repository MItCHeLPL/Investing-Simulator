using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StockListHolder
{
    public List<Stock> AllSavedStocks;
    public List<string> OwnedStockSymbols; //Improve to struct with stock, bought volume, price etc.


    //Get Stock from list if symbol exists
    public bool TryGetSavedStock(string symbol, out Stock stock)
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


    public void Serialize()
    {
        SystemIOJSONSerializer.Save<StockListHolder>("AllSavedStocks.json", this);
    }

    public bool TryDeserialize()
    {
        string path = SystemIOJSONSerializer.PathForFilename("AllSavedStocks.json");

        if (SystemIOJSONSerializer.FileExists(path))
        {
            this = SystemIOJSONSerializer.Load<StockListHolder>("AllSavedStocks.json");
            return true;
        }
        else
        {
            AllSavedStocks = new();
        }

        return false;
    }
}