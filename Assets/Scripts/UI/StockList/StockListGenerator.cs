using System.Collections;
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


    public virtual IEnumerator GenerateStockFields()
    {
        ClearFields();

        yield return new WaitUntil(() => stockHolder.HasGeneratedStocks);

        yield return new WaitForEndOfFrame();

        //Meant to be overriden
    }


    public void ClearFields()
    {
        UIUtils.ClearContent(transform);
    }

    protected virtual void GenerateField(Stock stock)
    {
        GameObject go = Instantiate(stockFieldPrefab, transform);

        StockField field = go.GetComponent<StockField>();

        field.Stock = stock;
        field.WindowManager = windowManager;

        field.Label.SetText(stock.Symbol);
        field.Value.SetText(((float)stock.CurrentValue).ToString());
    }
}
