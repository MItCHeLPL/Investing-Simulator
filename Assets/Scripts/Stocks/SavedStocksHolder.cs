using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct SavedStocksHolder
{
    public List<Stock> AllSavedStocks;


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
        AllSavedStocks = new();

        Serialize();
    }


    public void Serialize()
    {
        SystemIOJSONSerializer.Save<SavedStocksHolder>("AllSavedStocks.json", this);
    }

    public bool TryDeserialize()
    {
        string path = SystemIOJSONSerializer.PathForFilename("AllSavedStocks.json");

        if (SystemIOJSONSerializer.FileExists(path))
        {
            this = SystemIOJSONSerializer.Load<SavedStocksHolder>("AllSavedStocks.json");
            return true;
        }
        else
        {
            Clear();
        }

        return false;
    }
}