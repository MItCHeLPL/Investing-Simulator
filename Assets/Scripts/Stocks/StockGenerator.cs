using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockGenerator : MonoBehaviour
{
    public int Seed;
	public string AlphaVantageApiKey;

	public Stock GenerateAlphaVantageStock(string symbol)
	{
        //Other settings: https://www.alphavantage.co/documentation/
        //return AlphaVantageHandler.GetStock("TIME_SERIES_INTRADAY", symbol, "5min", AlphaVantageApiKey);
        return AlphaVantageHandler.GetStock("TIME_SERIES_INTRADAY", symbol, "30min", AlphaVantageApiKey);
	}

    public Stock GenerateAlphaVantageStock(string symbol, string interval)
    {
        //Other settings: https://www.alphavantage.co/documentation/
        //return AlphaVantageHandler.GetStock("TIME_SERIES_INTRADAY", symbol, "5min", AlphaVantageApiKey);
        return AlphaVantageHandler.GetStock("TIME_SERIES_INTRADAY", symbol, interval, AlphaVantageApiKey);
    }
}