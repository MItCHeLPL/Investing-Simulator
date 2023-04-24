using System;
using System.Collections.Generic;

[Serializable]
public struct OwnedStock
{
    public string Symbol;
    public List<StockValue> Shares;


    public OwnedStock(string symbol)
    {
        Symbol = symbol;
        Shares = new();
    }

    public OwnedStock(string symbol, List<StockValue> shares)
    {
        Symbol = symbol;
        Shares = shares;
    }


    public void AddShare(StockValue value)
    {
        Shares.Add(value);
    }

    public void RemoveShare(StockValue value)
    {
        Shares.Remove(value);
    }
    public void RemoveShare(int Id)
    {
        Shares.RemoveAt(Id);
    }
}