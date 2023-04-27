using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using UnityEngine.Events;

public class StockViewer : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private string _stockSymbol = "IBM";

	[Space(15)]

	[Tooltip("Refresh data every X seconds.")]
	[SerializeField] private float _dataRefreshRate = 5.0f;

	[Header("References")]
	[SerializeField] private StockGenerator stockGenerator;
	[SerializeField] private GraphGenerator graphGenerator;
	[SerializeField] private StockHolder stockHolder;

    [Space(15)]

	[SerializeField] private TextMeshProUGUI symbolText;
	[SerializeField] private TextMeshProUGUI currentValueText;

	[Space(15)]

	public static Stock CurrentStock = null;

	[Header("Coroutines")]
	private Coroutine _refreshingViewerCoroutine;
	private bool _isRefreshingViewerCoroutineRunning;

	[Header("Events")]
	public UnityEvent OnShow;


	private void OnEnable()
	{
		if (!_isRefreshingViewerCoroutineRunning)
		{
			_refreshingViewerCoroutine = StartCoroutine(RefreshingViewer());
			_isRefreshingViewerCoroutineRunning = true;
		}

    }
	private void OnDisable()
	{
		if (_isRefreshingViewerCoroutineRunning)
		{
			StopCoroutine(_refreshingViewerCoroutine);
			_isRefreshingViewerCoroutineRunning = false;
		}
	}


	[Button("Show Stock")]
	public void Show()
	{
		Show(_stockSymbol);
	}
	public void Show(string symbol)
	{
		Stock stock;

		int id = stockHolder.AllAvailableStockSymbols.FindIndex(x => x == symbol);

        if (id != -1)
		{
			stock = stockHolder.SavedStocksHolder.AllSavedStocks[id];
        }
		else
		{
			stock = stockGenerator.GenerateAlphaVantageStockInterday(symbol);
        }

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
			//UIUtils.ReplaceText(symbolText, "{StockName}", $"{stock.Symbol}");
			symbolText.SetText(stock.Symbol);
		}
		if (currentValueText != null)
		{
			//UIUtils.ReplaceText(currentValueText, "{StockCurrentValue}", $"{stock.CurrentValue.ToString()}");
			currentValueText.SetText($"${System.String.Format("{0:0.00}", stock.CurrentValue)}");
		}

		OnShow.Invoke();

		//Debug.Log($"Symbol: {stock.Symbol}, Values.Count: {stock.Values.Count}, CurrentValue: {stock.CurrentValue}, LowestValue: {stock.LowestCloseValue}, HighestValue: {stock.HighestCloseValue}");
	}


	public IEnumerator RefreshingViewer()
	{
		do
		{
			yield return new WaitForSecondsRealtime(_dataRefreshRate);

			if (CurrentStock != null)
			{
				Show(CurrentStock);
			}
		} while (_isRefreshingViewerCoroutineRunning);
	}
}
