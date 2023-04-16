using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockListGenerator : MonoBehaviour
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

        field.Label.SetText(stockSymbol);
        field.Value.SetText("Loading...");

        StartCoroutine(stockHolder.FillStockInfo(stockSymbol));

        field.button.interactable = false;

        yield return new WaitUntil(() => stockHolder.IsStockLoaded(stockSymbol));

        Stock stock = stockHolder.GetStockByStockSymbol(stockSymbol);

        field.Stock = stock;

        field.Value.SetText(((float)stock.CurrentValue).ToString());

        field.button.interactable = true;
    }
}
