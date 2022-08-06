using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

public class StockViewer : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private string _stockSymbol = "IBM";

	[Header("References")]
	[SerializeField] private StockGenerator stockGenerator;
	[SerializeField] private GraphGenerator graphGenerator;

	[Space(15)]

	[SerializeField] private TextMeshProUGUI symbolText;
	[SerializeField] private TextMeshProUGUI currentValueText;

	[Space(15)]

	public Stock CurrentStock;


	[Button("Show Stock")]
	public void Show()
	{
		Show(_stockSymbol);
	}
	public void Show(string symbol)
	{
		Stock stock = stockGenerator.GenerateAlphaVantageStock(symbol);

		Show(stock);
	}
	public void Show(Stock stock)
	{
		CurrentStock = stock;

		if (graphGenerator != null)
		{
			graphGenerator.GeneratePoints(stock);
		}
		if(symbolText != null)
		{
			symbolText.SetText(stock.Symbol);
		}
		if (currentValueText != null)
		{
			currentValueText.SetText(stock.CurrentValue.ToString());
		}

		Debug.Log($"Symbol: {stock.Symbol}, Values.Count: {stock.Values.Count}, CurrentValue: {stock.CurrentValue}, LowestValue: {stock.LowestValue}, HighestValue: {stock.HighestValue}");
	}
}
