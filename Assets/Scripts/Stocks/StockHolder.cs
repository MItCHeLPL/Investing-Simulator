using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public class StockHolder : MonoBehaviour
{
    [SerializeField] private StockGenerator stockGenerator;
    [ReadOnly] public StockListHolder StockListHolder;

    public List<string> AllAvailableStockSymbols = new List<string>();

    [HideInInspector] public bool HasGeneratedStocks = false;

    private Timer loadTimer = new Timer();
    [SerializeField] private float maxLoadTime = 5.0f;


    private void Start()
    {
        StockListHolder.TryDeserialize();

        StartCoroutine(FillStockInfo());
    }


    private IEnumerator FillStockInfo()
    {
        loadTimer.StartTimer();

        for (int i = 0; i < AllAvailableStockSymbols.Count; i++)
        {
            string stockSymbol = AllAvailableStockSymbols[i];

            if(StockListHolder.TryGetSavedStock(stockSymbol, out Stock stock)) //if stock with this symbol is in SO
            {
                TimeSpan generationTimeDiff = DateTime.Now.Subtract(DateTime.FromBinary(stock.GenerateTime));

                Debug.Log(generationTimeDiff.TotalMinutes);

                //Generate new data into stock if current data in SO is older than api refresh time
                if (AlphaVantageTimer.IsApiCallAllowed && generationTimeDiff.TotalMinutes > 15.0f) //15min
                {
                    try
                    {
                        Stock newData = stockGenerator.GenerateAlphaVantageStock(stockSymbol);

                        int newEntryCount = 0;

                        DateTime oldDataLastTimestamp = DateTime.FromBinary(stock.Values[^1].TimestampBinary); //save last timestamp from old data

                        for (int j = 0; j < newData.Values.Count; j++)
                        {
                            DateTime newDataTimestamp = DateTime.FromBinary(newData.Values[j].TimestampBinary); //timestamp from new data

                            //if new data is newer than old data, add it to the old data and save
                            TimeSpan dataTimeDiff = newDataTimestamp.Subtract(oldDataLastTimestamp);

                            if (dataTimeDiff.Ticks < 0) //new data is newer than last in old data
                            {
                                stock.Values.Add(newData.Values[j]);
                                newEntryCount++;
                            }
                        }

                        //Save stock with new data for serialization
                        StockListHolder.AllSavedStocks[StockListHolder.AllSavedStocks.FindIndex(x => x.Symbol == stock.Symbol)] = new(stock);


                        Debug.Log($"{stockSymbol} - Genereted new data. Trying to add into SavedStocks, new entries: {newEntryCount}");
                    }
                    catch
                    {
                        Debug.Log($"{stockSymbol} - Can't get new stock data, api call error, Loaded old data from SavedStocks");
                    }
                }

                //Get stock data from SO
                else
                {
                    Debug.Log($"{stockSymbol} - Loaded from SavedStocks");
                }
            }

            //Stock isn't in SO
            else
            {
                //Generate new data into SO
                if (AlphaVantageTimer.IsApiCallAllowed)
                {
                    try
                    {
                        Stock outputStock = stockGenerator.GenerateAlphaVantageStock(stockSymbol);

                        StockListHolder.AllSavedStocks.Add(outputStock);

                        Debug.Log($"{stockSymbol} - Generated data, created new entry in SavedStocks");
                    }
                    //Error used out api resources
                    catch
                    {
                        Debug.Log($"{stockSymbol} - Can't get stock data, api call error");
                    }
                }

                //Over API call limit
                else
                {
                    Debug.LogError($"{stockSymbol} - Can't get stock data, api call not allowed");
                }
            }
        }

        yield return new WaitUntil(() => StockListHolder.AllSavedStocks[^1].Values.Count > 0 || loadTimer.GetTime() > maxLoadTime); //Wait for stock load

        StockListHolder.Serialize();

        HasGeneratedStocks = true;
    }
}
