using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StockListGenerator : MonoBehaviour
{
    [SerializeField] protected GameObject stockFieldPrefab;
    [SerializeField] protected StockHolder stockHolder;
    [SerializeField] protected WindowManager windowManager;


    protected virtual void OnEnable()
    {
        StartCoroutine(GenerateStockFields());
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }


    public virtual IEnumerator GenerateStockFields()
    {
        yield return new WaitForEndOfFrame(); //wait for deserialzation

        UIUtils.ClearContent(transform);

        //Meant to be overriden
    }

    protected virtual IEnumerator GenerateField(string stockSymbol)
    {
        GameObject go = Instantiate(stockFieldPrefab, transform);

        StockField field = go.GetComponent<StockField>();

        field.WindowManager = windowManager;

        field.Name.SetText(stockSymbol);
        field.FullName.SetText(stockHolder.AllAvailableStockSymbolsAndNames[stockSymbol]);

        field.Value.SetText("Loading...");

        StartCoroutine(stockHolder.FillStockInfo(stockSymbol));

        field.button.interactable = false;

        yield return new WaitUntil(() => stockHolder.IsStockLoaded(stockSymbol));

        Stock stock = stockHolder.GetStockByStockSymbol(stockSymbol);

        field.Stock = stock;

        field.Value.SetText($"${System.String.Format("{0:0.00}", stock.CurrentValue)}");


        string balanceChar = "<color=black>";
        if (stock.Balance24h > 0) balanceChar = "<color=#008800>+";
        else if (stock.Balance24h < 0) balanceChar = "<color=red>-";

        field.Balance.SetText($"{balanceChar}${System.String.Format("{0:0.00}", Mathf.Abs((float)stock.Balance24h))}</color>");


        if (stockHolder.OwnedStocksHolder.TryGetOwnedStock(stockSymbol, out OwnedStock ownedStock))
        {
            int amount = ownedStock.Shares.Count;

            field.Amount.SetText($"{amount} owned");
        }
        else
        {
            field.Amount.SetText($"0 owned");
        }


        field.button.interactable = true;
    }
}
