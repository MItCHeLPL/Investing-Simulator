using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class StockViewer : MonoBehaviour
{
	[SerializeField] private StockGenerator _stockGenerator;

	[SerializeField] private string _stockSymbol = "IBM";


	[Button("Show Stock")]
	public void Show()
	{
		Show(_stockSymbol);
	}
	public void Show(string symbol)
	{
		Stock stock = _stockGenerator.GenerateAlphaVantageStock(symbol);

		Debug.Log($"Symbol: {stock.Symbol}, Values.Count: {stock.Values.Count}, CurrentValue: {stock.CurrentValue}, HighestValue: {stock.HighestValue}, LowestValue: {stock.LowestValue}");
	}
	public void Show(Stock stock)
	{
		//TODO
	}
}
