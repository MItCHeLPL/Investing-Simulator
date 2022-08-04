using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockGenerator : MonoBehaviour
{
    public int Seed;
	public string AlphaVantageApiKey;


	public Stock GenerateStock(int seed, string name, double existsFor)
	{
		Random.InitState(seed);
		return GenerateStock(seed, Random.Range(0, 1), name, existsFor);
	}
	public Stock GenerateStock(int seed, double stability, string name, double existsFor)
	{
		Random.InitState(seed);

		//TODO

		return null;
	}

	public Stock GenerateAlphaVantageStock(string symbol)
	{
		//Other settings: https://www.alphavantage.co/documentation/
		return AlphaVantageHandler.GetStock("TIME_SERIES_INTRADAY", symbol, "5min", AlphaVantageApiKey);
	}
}