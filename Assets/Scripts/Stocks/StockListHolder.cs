using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StockListHolder
{
    public List<Stock> AllSavedStocks;


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


    public void Serialize()
    {
        string json = JsonUtility.ToJson(this);

        Debug.Log(json.Length);

        PlayerPrefs.SetString("AllSavedStocks", json); //TODO Change to SystemIO
    }

    public bool TryDeserialize()
    {
        if (PlayerPrefs.HasKey("AllSavedStocks"))
        {
            string json = PlayerPrefs.GetString("AllSavedStocks"); //TODO Change to SystemIO

            this = JsonUtility.FromJson<StockListHolder>(json);

            return true;
        }

        else
        {
            AllSavedStocks = new();
        }

        return false;
    }
}