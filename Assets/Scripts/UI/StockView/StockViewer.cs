using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using UnityEngine.Events;
using System.Linq;

public class StockViewer : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private string _stockSymbol = "IBM";

	[Space(15)]

	[Tooltip("Refresh data every X seconds.")]
	[SerializeField] private float _dataRefreshRate = 15.0f;

	[Header("References")]
	[SerializeField] private StockGenerator stockGenerator;
	[SerializeField] private GraphGenerator graphGenerator;
	[SerializeField] private StockHolder stockHolder;
	[SerializeField] private TransactionsController transactionsController;

    [Space(15)]

	[SerializeField] private TextMeshProUGUI symbolText;
	[SerializeField] private TextMeshProUGUI currentValueText;
	[SerializeField] private TextMeshProUGUI currentBalanceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI detailsText;

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

        string key = stockHolder.AllAvailableStockSymbolsAndNames.Keys.FirstOrDefault(x => x == symbol);

        if (!string.IsNullOrEmpty(key))
		{
			stock = stockHolder.SavedStocksHolder.AllSavedStocks.Find(x => x.Symbol == key);
        }
		else
		{
			stock = stockGenerator.GenerateAlphaVantageStockInterday(symbol);
        }

		Show(stock);
	}
	public void Show(Stock stock)
	{
        _stockSymbol = stock.Symbol;

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
            currentValueText.SetText($"${System.String.Format("{0:0.00}", stock.CurrentValue)}");

            string balanceChar = "<color=#808080>";
            if (stock.Balance24h > 0) balanceChar = "<color=#008800>+";
            else if (stock.Balance24h < 0) balanceChar = "<color=red>-";

            currentBalanceText.SetText($"{balanceChar}${System.String.Format("{0:0.00}", Mathf.Abs((float)stock.Balance24h))}</color>");
        }
		if (nameText != null)
		{
			nameText.SetText($"{stockHolder.AllAvailableStockSymbolsAndNames.FirstOrDefault(x => x.Key == stock.Symbol).Value}");
        }
        if (detailsText != null)
        {
			int amount = 0;
            if (transactionsController.stockHolder.OwnedStocksHolder.TryGetOwnedStock(stock.Symbol, out OwnedStock ownedStock))
            {
				amount = ownedStock.Shares.Count;
            }

			double balance = transactionsController.GetBalanceForStock(stock.Symbol);

            string balanceChar = "<color=#808080>";
            if (balance > 0) balanceChar = "<color=#008800>+";
            else if (balance < 0) balanceChar = "<color=red>-";

            string content = $"Owned stock amount: {amount}\n" +
				$"Money in this stock shares: ${System.String.Format("{0:0.00}", stock.CurrentValue * amount)}\n" +
				$"Profit / Loss: {balanceChar}${System.String.Format("{0:0.00}", Mathf.Abs((float)balance))}</color>";

            detailsText.SetText(content);
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
