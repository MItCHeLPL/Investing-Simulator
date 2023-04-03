using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StockHolder : MonoBehaviour
{
    [SerializeField] private StockGenerator stockGenerator;
    [SerializeField] private StockSOsHolder soHolder;

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

        for (int i = 0; i < AllStockSymbols.Count; i++)
        {
            Stock outputStock = null;

            if(soHolder.TryGetStock(AllStockSymbols[i], out Stock stock)) //if stock with this symbol is in SO
            {
                TimeSpan timeDiff = DateTime.Now.Subtract(DateTime.FromBinary(stock.GenerateTime));

                //Generate new data into stock if current data in SO is older than api refresh time
                if (AlphaVantageTimer.IsApiCallAllowed && timeDiff.TotalMinutes > 15.0f) //15min
                {
                    outputStock = stockGenerator.GenerateAlphaVantageStock(AllStockSymbols[i]);

                    soHolder.AllSavedStocks[soHolder.AllSavedStocks.FindIndex(x => x == stock)] = outputStock;


                    Debug.Log($"{outputStock.Symbol} - Genereted new data into SO");
                }

                //Get stock data from SO
                else
                {
                    outputStock = stock;


                    Debug.Log($"{outputStock.Symbol} - Loaded from SO");
                }
            }

            //Stock isn't in SO
            else
            {
                //Generate new data into SO
                if (AlphaVantageTimer.IsApiCallAllowed)
                {
                    outputStock = stockGenerator.GenerateAlphaVantageStock(AllStockSymbols[i]);

                    soHolder.AllSavedStocks.Add(outputStock);


                    Debug.Log($"{outputStock.Symbol} - Created new entry in SO from generated data");
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

        HasGeneratedStocks = true;
    }
}
