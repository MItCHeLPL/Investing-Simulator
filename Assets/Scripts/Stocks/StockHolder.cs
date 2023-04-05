using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public class StockHolder : MonoBehaviour
{
    [SerializeField] private StockGenerator stockGenerator;
    [SerializeField] [ReadOnly] private StockListHolder stockListHolder;

    public List<string> AllStockSymbols = new List<string>();

    [HideInInspector] public List<Stock> AllStocks = new List<Stock>();

    public List<int> OwnedStocksFromAll = new List<int>();

    [HideInInspector] public bool HasGeneratedStocks = false;

    private Timer loadTimer = new Timer();
    [SerializeField] private float maxLoadTime = 5.0f;


    private void Start()
    {
        StartCoroutine(FillStockInfo());
    }


    private IEnumerator FillStockInfo()
    {
        AllStocks = new();
        loadTimer.StartTimer();

        stockListHolder.TryDeserialize();

        for (int i = 0; i < AllStockSymbols.Count; i++)
        {
            Stock outputStock = null;

            if(stockListHolder.TryGetStock(AllStockSymbols[i], out Stock stock)) //if stock with this symbol is in SO
            {
                TimeSpan timeDiff = DateTime.Now.Subtract(DateTime.FromBinary(stock.GenerateTime));

                //Generate new data into stock if current data in SO is older than api refresh time
                if (AlphaVantageTimer.IsApiCallAllowed && timeDiff.TotalMinutes > 15.0f) //15min
                {
                    outputStock = stockGenerator.GenerateAlphaVantageStock(AllStockSymbols[i]);

                    stockListHolder.AllSavedStocks[stockListHolder.AllSavedStocks.FindIndex(x => x == stock)] = outputStock;


                    Debug.Log($"{outputStock.Symbol} - Genereted new data into SavedStocks");
                }

                //Get stock data from SO
                else
                {
                    outputStock = stock;


                    Debug.Log($"{outputStock.Symbol} - Loaded from SavedStocks");
                }
            }

            //Stock isn't in SO
            else
            {
                //Generate new data into SO
                if (AlphaVantageTimer.IsApiCallAllowed)
                {
                    outputStock = stockGenerator.GenerateAlphaVantageStock(AllStockSymbols[i]);

                    stockListHolder.AllSavedStocks.Add(outputStock);


                    Debug.Log($"{outputStock.Symbol} - Created new entry in SavedStocks from generated data");
                }

                //Over API call limit
                else
                {
                    Debug.LogError($"Can't get stock data");
                }
            }

            AllStocks.Add(outputStock);
        }

        yield return new WaitUntil(() => AllStocks[^1].Values.Count > 0 || loadTimer.GetTime() > maxLoadTime); //Wait for stock load

        stockListHolder.Serialize();

        HasGeneratedStocks = true;
    }
}
