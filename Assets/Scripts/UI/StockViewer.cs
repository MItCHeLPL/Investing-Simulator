using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockViewer : MonoBehaviour
{
	[SerializeField] private StockGenerator stockGenerator;


	private void Start()
	{
		Show("IBM");
	}

	public void Show(string symbol)
	{
		Stock stock = stockGenerator.GenerateAlphaVantageStock(symbol);

		Debug.Log($"{stock.Symbol}, {stock.Values.Count}, {stock.CurrentValue}, {stock.HighestValue}, {stock.LowestValue}");
	}
	public void Show(Stock stock)
	{
		//TODO
	}
}
