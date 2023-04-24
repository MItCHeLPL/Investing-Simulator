using UnityEngine;

public class TransactionsController : MonoBehaviour
{
    [SerializeField] private StockHolder stockHolder;

    public double OwnedMoney => stockHolder.OwnedStocksHolder.OwnedMoney;
    public double WorthFromOwnedStocks => GetWorthFromOwnedStocks();
    public double TotalWorth => GetTotalWorth();


    [ContextMenu(itemName: "Add500")]
    public void Add500() => AddMoney(500.0d);
    public void AddMoney(double amount)
    {
        stockHolder.OwnedStocksHolder.OwnedMoney += amount;

        stockHolder.OwnedStocksHolder.Serialize();
    }

    [ContextMenu("Sub500")]
    public void Sub500() => SubtractMoney(500.0d);
    public void SubtractMoney(double amount)
    {
        if(stockHolder.OwnedStocksHolder.OwnedMoney - amount >= 0)
        {
            stockHolder.OwnedStocksHolder.OwnedMoney -= amount;
        }
        else
        {
            stockHolder.OwnedStocksHolder.OwnedMoney = 0;
        }

        stockHolder.OwnedStocksHolder.Serialize();
    }

    public void SetMoney(double amount)
    {
        if(amount >= 0)
        {
            stockHolder.OwnedStocksHolder.OwnedMoney = amount;

            stockHolder.OwnedStocksHolder.Serialize();
        }
    }


    public double GetWorthFromOwnedStocks()
    {
        double worth = 0;

        foreach(var x in stockHolder.OwnedStocksHolder.OwnedStocks)
        {
            if(stockHolder.SavedStocksHolder.TryGetSavedStock(x.Symbol, out Stock stock))
            {
                worth += stock.CurrentValue * x.Shares.Count;
            }
        }

        return worth;
    }
    public double GetTotalWorth()
    {
        return GetWorthFromOwnedStocks() + OwnedMoney;
    }


    public void BuyShares(string stockSymbol, StockValue stockValue, int amount)
    {
        if (stockHolder.OwnedStocksHolder.TryGetOwnedStock(stockSymbol, out OwnedStock ownedStock))
        {
            if (OwnedMoney >= stockValue.Close * amount)
            {
                ownedStock.AddShare(stockValue);

                SubtractMoney(stockValue.Close * amount);

                stockHolder.OwnedStocksHolder.Serialize();


                Debug.Log($"Bought {amount} stocks for: {stockValue.Close * amount}");
            }
            else
            {
                Debug.Log($"Can't buy, not enough money, current money: {OwnedMoney}");
            }
        }
        else
        {
            if (OwnedMoney >= stockValue.Close * amount)
            {
                OwnedStock newOwnedStock = new(stockSymbol);

                newOwnedStock.AddShare(stockValue);

                SubtractMoney(stockValue.Close * amount);

                stockHolder.OwnedStocksHolder.OwnedStocks.Add(newOwnedStock);

                stockHolder.OwnedStocksHolder.Serialize();


                Debug.Log($"Bought {amount} stocks for: {stockValue.Close * amount}");
            }
            else
            {
                Debug.Log($"Can't buy, not enough money, current money: {OwnedMoney}");
            }
        }
    }

    public void SellShares(string stockSymbol, StockValue stockValue, int amount)
    {
        if (stockHolder.OwnedStocksHolder.TryGetOwnedStock(stockSymbol, out OwnedStock ownedStock))
        {
            if (ownedStock.Shares.Count >= amount)
            {
                ownedStock.RemoveShare(0);

                AddMoney(stockValue.Close * amount);

                if (ownedStock.Shares.Count == 0)
                {
                    stockHolder.OwnedStocksHolder.OwnedStocks.Remove(ownedStock);
                }

                stockHolder.OwnedStocksHolder.Serialize();


                Debug.Log($"Sold {amount} stocks for: {stockValue.Close * amount}");
            }
            else
            {
                Debug.Log($"Can't sell, not enough shares, share count for this stock: {ownedStock.Shares.Count}");
            }
        }
        else
        {
            Debug.Log("Stock not owned");
        }
    }
}
