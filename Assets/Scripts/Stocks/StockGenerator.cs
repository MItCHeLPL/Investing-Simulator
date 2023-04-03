using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockGenerator : MonoBehaviour
{
    public int Seed;
	public string AlphaVantageApiKey;

	public Stock GenerateAlphaVantageStock(string symbol)
	{
        return GenerateAlphaVantageStockInterday(symbol);
    }

    public Stock GenerateAlphaVantageStockInterday(string symbol)
    {
        return GenerateAlphaVantageStock("TIME_SERIES_INTRADAY", symbol, "15min");
    }

    public Stock GenerateAlphaVantageStockWeekly(string symbol)
    {
        return GenerateAlphaVantageStock("TIME_SERIES_WEEKLY", symbol, "");
    }

    public Stock GenerateAlphaVantageStock(string function, string symbol, string interval)
    {
        //Other settings: https://www.alphavantage.co/documentation/
        //return AlphaVantageHandler.GetStock("TIME_SERIES_INTRADAY", symbol, "5min", AlphaVantageApiKey);
        return AlphaVantageHandler.GetStock(function, symbol, interval, AlphaVantageApiKey);
    }
}