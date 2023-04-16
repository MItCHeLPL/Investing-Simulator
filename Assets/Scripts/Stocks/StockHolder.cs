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
                TimeSpan generationTimeDiff = DateTime.Now.Subtract(DateTime.FromBinary(stock.GenerateTime));

                //Generate new data into stock if current data in SO is older than api refresh time
                if (AlphaVantageTimer.IsApiCallAllowed && generationTimeDiff.TotalMinutes > 15.0f) //15min
                {
                    int newEntryCount = 0;

                    Stock newData = stockGenerator.GenerateAlphaVantageStock(AllStockSymbols[i]);

                    long oldDataLastTimestamp = stock.Values[^1].TimestampBinary; //save last timestamp from old data

                    for (int j = 0; j < newData.Values.Count; j++)
                    {
                        long newDataTimestamp = newData.Values[j].TimestampBinary; //timestamp from new data

                        //if new data is newer than old data, add it to the old data and save
                        TimeSpan dataTimeDiff = DateTime.FromBinary(newDataTimestamp).Subtract(DateTime.FromBinary(oldDataLastTimestamp)); 

                        if (dataTimeDiff.Ticks < 0)
                        {
                            stock.Values.Add(newData.Values[j]);
                            newEntryCount++;
                        }
                    }

                    outputStock = stock;


                    Debug.Log($"{outputStock.Symbol} - Genereted new data into SavedStocks. New entries: {newEntryCount}");
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
