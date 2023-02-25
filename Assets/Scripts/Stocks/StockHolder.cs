using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockHolder : MonoBehaviour
{
    [SerializeField] private StockGenerator stockGenerator;

    public List<string> AllStockSymbols = new List<string>();

    [HideInInspector] public List<Stock> AllStocks = new List<Stock>();

    public List<int> OwnedStocksFromAll = new List<int>();

    [HideInInspector] public bool HasGeneratedStocks = false;

    private Timer loadTimer = new Timer();
    [SerializeField] private float maxLoadTime = 5.0f;


    private void Start()
    {
        StartCoroutine(FillStockInfo());
    }


    private IEnumerator FillStockInfo()
    {
        AllStocks = new();
        loadTimer.StartTimer();

        for (int i = 0; i < AllStockSymbols.Count; i++)
        {
            //TODO: Add SO with historic stock data
            Stock stock = stockGenerator.GenerateAlphaVantageStock(AllStockSymbols[i]);
            AllStocks.Add(stock);
        }

        yield return new WaitUntil(() => AllStocks[^1].Values.Count > 0 || loadTimer.GetTime() > maxLoadTime);

        HasGeneratedStocks = true;
    }
}
