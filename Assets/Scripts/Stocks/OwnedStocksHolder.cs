using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct OwnedStocksHolder
{
    public List<OwnedStock> OwnedStocks;
    public double OwnedMoney;
    public double StartOwnedMoney;

    public static UnityEvent OnOwnedMoneyChange;


    public OwnedStocksHolder(double startMoney)
    {
        OwnedStocks = new();
        OwnedMoney = startMoney;
        StartOwnedMoney = startMoney;
        OnOwnedMoneyChange = new();

        OnOwnedMoneyChange.Invoke();
    }


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


    public void AddMoney(double amount)
    {
        OwnedMoney += amount;

        Serialize();

        OnOwnedMoneyChange.Invoke();
    }

    public void SubtractMoney(double amount)
    {
        if (OwnedMoney - amount >= 0)
        {
            OwnedMoney -= amount;
        }
        else
        {
            OwnedMoney = 0;
        }

        Serialize();

        OnOwnedMoneyChange.Invoke();
    }

    public void SetMoney(double amount)
    {
        if (amount >= 0)
        {
            OwnedMoney = amount;

            Serialize();

            OnOwnedMoneyChange.Invoke();
        }
    }


    [ContextMenu("Clear")]
    public void Clear()
    {
        OwnedStocks = new();
        OwnedMoney = 0;

        OnOwnedMoneyChange.Invoke();

        Serialize();
    }

    [ContextMenu("SetStartValues")]
    public void SetStartValues()
    {
        OwnedStocks = new();
        OwnedMoney = StartOwnedMoney;

        OnOwnedMoneyChange.Invoke();

        Serialize();
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

            OnOwnedMoneyChange.Invoke();

            return true;
        }
        else
        {
            SetStartValues();
        }

        return false;
    }
}