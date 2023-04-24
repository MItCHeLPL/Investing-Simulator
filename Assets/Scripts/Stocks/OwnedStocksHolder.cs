using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct OwnedStocksHolder
{
    public List<OwnedStock> OwnedStocks;
    public double OwnedMoney;


    public bool TryGetOwnedStock(string symbol, out OwnedStock ownedStock)
    {
        ownedStock = new();

        for (int j = 0; j < OwnedStocks.Count; j++)
        {
            OwnedStock s = OwnedStocks[j];

            if (s.Symbol == symbol)
            {
                ownedStock = s;

                return true;
            }
        }

        return false;
    }


    [ContextMenu("Clear")]
    public void Clear()
    {
        OwnedStocks = new();
        OwnedMoney = 0;
    }

    [ContextMenu("SetStartValues")]
    public void SetStartValues()
    {
        OwnedStocks = new();
        OwnedMoney = 5000.0d;
    }


    public void Serialize()
    {
        SystemIOJSONSerializer.Save<OwnedStocksHolder>("OwnedSavedStocks.json", this);
    }

    public bool TryDeserialize()
    {
        string path = SystemIOJSONSerializer.PathForFilename("OwnedSavedStocks.json");

        if (SystemIOJSONSerializer.FileExists(path))
        {
            this = SystemIOJSONSerializer.Load<OwnedStocksHolder>("OwnedSavedStocks.json");
            return true;
        }
        else
        {
            SetStartValues();
        }

        return false;
    }
}