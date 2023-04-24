using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public class StockHolder : MonoBehaviour
{
    [SerializeField] private StockGenerator stockGenerator;
    [ReadOnly] public SavedStocksHolder SavedStocksHolder;
    [ReadOnly] public OwnedStocksHolder OwnedStocksHolder;

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
        SavedStocksHolder.TryDeserialize();
        OwnedStocksHolder.TryDeserialize();
    }


    public IEnumerator FillStockInfo(string stockSymbol)
    {
        while(ShouldStockReload(stockSymbol))
        {
            if (SavedStocksHolder.TryGetSavedStock(stockSymbol, out Stock stock)) //if stock with this symbol is in SO
            {
                TimeSpan generationTimeDiff = DateTime.Now.Subtract(DateTime.FromBinary(stock.GenerateTime));

                //Generate new data into stock if current data in SO is older than api refresh time
                if (generationTimeDiff.TotalMinutes > 15.0f) //15min
                {
                    FillNewData(stock);
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

                        SavedStocksHolder.AllSavedStocks.Add(outputStock);

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

        SavedStocksHolder.Serialize();
    }

    private void FillNewData(Stock stock)
    {
        try
        {
            Stock newData = stockGenerator.GenerateAlphaVantageStock(stock.Symbol);

            int newEntryCount = 0;

            DateTime oldDataLastTimestamp = DateTime.FromBinary(stock.Values[0].TimestampBinary); //save last timestamp from old data

            for (int j = newData.Values.Count - 1; j >= 0; j--)
            {
                DateTime newDataTimestamp = DateTime.FromBinary(newData.Values[j].TimestampBinary); //timestamp from new data

                //if new data is newer than old data, add it to the old data and save
                TimeSpan dataTimeDiff = newDataTimestamp.Subtract(oldDataLastTimestamp);

                if (dataTimeDiff.Ticks > 0) //new data is newer than last in old data
                {
                    stock.Values.Insert(0, newData.Values[j]); //insert new data at the start of list
                    newEntryCount++;
                }
            }

            //Save stock with new data for serialization
            SavedStocksHolder.AllSavedStocks[SavedStocksHolder.AllSavedStocks.FindIndex(x => x.Symbol == stock.Symbol)] = new(stock);

            stockSymbolLoaded[stock.Symbol] = LoadStatus.AddedNewData;

            Debug.Log($"{stock.Symbol} - Genereted new data. Trying to add into SavedStocks, new entries: {newEntryCount}");
        }
        catch
        {
            stockSymbolLoaded[stock.Symbol] = LoadStatus.APICallOnCooldownLoadedOldData;

            Debug.Log($"{stock.Symbol} - Can't get new stock data, api call error, Loaded old data from SavedStocks");
        }
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
        return SavedStocksHolder.AllSavedStocks[SavedStocksHolder.AllSavedStocks.FindIndex(x => x.Symbol == stockSymbol)];
    }
}
