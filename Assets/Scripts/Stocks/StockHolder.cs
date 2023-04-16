using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public class StockHolder : MonoBehaviour
{
    [SerializeField] private StockGenerator stockGenerator;
    [ReadOnly] public StockListHolder StockListHolder;

    public List<string> AllAvailableStockSymbols = new();

    private Dictionary<string, LoadStatus> stockSymbolLoaded = new();

    [HideInInspector] public bool HasGeneratedAllStocks = false;

    [SerializeField] private float maxLoadTime = 10.0f;


    [Flags]
    public enum LoadStatus
    {
        LoadedOldData = 1,
        CreatedNewData = 2,
        AddedNewData = 4,

        APICallOnCooldown = 8,
        APICallOnCooldownLoadedOldData = APICallOnCooldown | LoadedOldData,
    }


    private void Awake()
    {
        StockListHolder.TryDeserialize();
    }


    public IEnumerator FillStockInfo(string stockSymbol)
    {
        while(ShouldStockReload(stockSymbol))
        {
            if (StockListHolder.TryGetSavedStock(stockSymbol, out Stock stock)) //if stock with this symbol is in SO
            {
                TimeSpan generationTimeDiff = DateTime.Now.Subtract(DateTime.FromBinary(stock.GenerateTime));

                //Generate new data into stock if current data in SO is older than api refresh time
                if (generationTimeDiff.TotalMinutes > 15.0f) //15min
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

                        stockSymbolLoaded[stockSymbol] = LoadStatus.AddedNewData;

                        Debug.Log($"{stockSymbol} - Genereted new data. Trying to add into SavedStocks, new entries: {newEntryCount}");
                    }
                    catch
                    {
                        stockSymbolLoaded[stockSymbol] = LoadStatus.APICallOnCooldownLoadedOldData;

                        Debug.Log($"{stockSymbol} - Can't get new stock data, api call error, Loaded old data from SavedStocks");
                    }
                }

                //Get stock data from SO
                else
                {
                    stockSymbolLoaded[stockSymbol] = LoadStatus.LoadedOldData;

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

                        stockSymbolLoaded[stockSymbol] = LoadStatus.AddedNewData;

                        Debug.Log($"{stockSymbol} - Generated data, created new entry in SavedStocks");
                    }
                    //Error used out api resources
                    catch
                    {
                        stockSymbolLoaded[stockSymbol] = LoadStatus.APICallOnCooldown;

                        Debug.Log($"{stockSymbol} - Can't get stock data, api call error");
                    }
                }

                //Over API call limit
                else
                {
                    stockSymbolLoaded[stockSymbol] = LoadStatus.APICallOnCooldown;

                    Debug.LogError($"{stockSymbol} - Can't get stock data, api call not allowed");
                }
            }


            if(ShouldStockReload(stockSymbol))
                yield return new WaitForSeconds(maxLoadTime);
        }

        StockListHolder.Serialize();
    }


    public bool IsStockLoaded(string stockSymbol)
    {
        return stockSymbolLoaded.ContainsKey(stockSymbol) && (
            stockSymbolLoaded[stockSymbol] == LoadStatus.LoadedOldData ||
            stockSymbolLoaded[stockSymbol] == LoadStatus.CreatedNewData ||
            stockSymbolLoaded[stockSymbol] == LoadStatus.AddedNewData ||
            stockSymbolLoaded[stockSymbol] == LoadStatus.APICallOnCooldownLoadedOldData
        );
    }
    public bool ShouldStockReload(string stockSymbol)
    {
        return !stockSymbolLoaded.ContainsKey(stockSymbol) ||
            stockSymbolLoaded[stockSymbol] == LoadStatus.APICallOnCooldown ||
            stockSymbolLoaded[stockSymbol] == LoadStatus.APICallOnCooldownLoadedOldData;     
    }


    public Stock GetStockByStockSymbol(string stockSymbol)
    {
        return StockListHolder.AllSavedStocks[StockListHolder.AllSavedStocks.FindIndex(x => x.Symbol == stockSymbol)];
    }
}
