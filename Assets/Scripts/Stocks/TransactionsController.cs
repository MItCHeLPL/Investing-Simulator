using UnityEngine;

public class TransactionsController : MonoBehaviour
{
    public StockHolder stockHolder;

    public double OwnedMoney => stockHolder.OwnedStocksHolder.OwnedMoney;
    public double WorthFromOwnedStocks => GetWorthFromOwnedStocks();
    public double TotalWorth => GetTotalWorth();


    [ContextMenu(itemName: "Add500")]
    public void Add500() => AddMoney(500.0d);
    [ContextMenu("Sub500")]
    public void Sub500() => SubtractMoney(500.0d);

    public void AddMoney(double amount) => stockHolder.OwnedStocksHolder.AddMoney(amount);
    public void SubtractMoney(double amount) => stockHolder.OwnedStocksHolder.SubtractMoney(amount);
    public void SetMoney(double amount) => stockHolder.OwnedStocksHolder.SetMoney(amount);


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
                for(int i=0; i< amount; i++)
                {
                    ownedStock.AddShare(stockValue);
                }

                SubtractMoney(stockValue.Close * amount);

                stockHolder.OwnedStocksHolder.Serialize();


                Debug.Log($"Bought {amount} stocks for: {stockValue.Close * amount}");
            }
            else
            {
                InfoPopup.Show($"Can't buy, not enough money");
                Debug.Log($"Can't buy, not enough money, current money: {OwnedMoney}");
            }
        }
        else
        {
            if (OwnedMoney >= stockValue.Close * amount)
            {
                OwnedStock newOwnedStock = new(stockSymbol);

                for (int i = 0; i < amount; i++)
                {
                    newOwnedStock.AddShare(stockValue);
                }

                SubtractMoney(stockValue.Close * amount);

                stockHolder.OwnedStocksHolder.OwnedStocks.Add(newOwnedStock);

                stockHolder.OwnedStocksHolder.Serialize();


                Debug.Log($"Bought {amount} stocks for: {stockValue.Close * amount}");
            }
            else
            {
                InfoPopup.Show($"Can't buy, not enough money");
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
                for (int i = 0; i < amount; i++)
                {
                    ownedStock.RemoveShare(0);
                }

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
                InfoPopup.Show($"Can't sell, not enough shares.\nOwned shares amount of this stock: {ownedStock.Shares.Count}");
                Debug.Log($"Can't sell, not enough shares, share count for this stockValue: {ownedStock.Shares.Count}");
            }
        }
        else
        {
            InfoPopup.Show("Can't sell, stock not owned");
            Debug.Log("Stock not owned");
        }
    }

    public double GetTotalBalance()
    {
        double balance = 0;

        foreach(var ownedStock in stockHolder.OwnedStocksHolder.OwnedStocks)
        {
            Stock stock = stockHolder.SavedStocksHolder.AllSavedStocks.Find(x => x.Symbol == ownedStock.Symbol);

            if (stock != null)
            {
                foreach (var stockValue in ownedStock.Shares)
                {
                    balance += (stock.CurrentValue - stockValue.Close);
                }
            }
        }

        return balance;
    }

    public double GetBalanceForStock(string stockSymbol)
    {
        double balance = 0;

        Stock stock = stockHolder.SavedStocksHolder.AllSavedStocks.Find(x => x.Symbol == stockSymbol);

        if (stockHolder.OwnedStocksHolder.TryGetOwnedStock(stockSymbol, out OwnedStock ownedStock))
        {
            if(stock != null)
            {
                foreach (var stockValue in ownedStock.Shares)
                {
                    balance += (stock.CurrentValue - stockValue.Close);
                }
            }
        }

        return balance;
    }
}
