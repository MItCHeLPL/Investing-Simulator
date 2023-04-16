using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private StockViewer stockViewer;
    [SerializeField] private GameObject allStockList;
    [SerializeField] private GameObject ownedStockList;

    private Stock lastStock = null;

    private Panel currentPanel = Panel.None;
    private Panel prevPanel = Panel.None;
    

    public enum Panel
    {
        None,
        AllStocks,
        OwnedStocks,
        StockPanel
    }


    private void Start()
    {
        ShowAllStocks(); //Default window
    }


    public void HideAllPanels()
    {
        stockViewer.gameObject.SetActive(false);
        allStockList.SetActive(false);
        ownedStockList.SetActive(false);
    }


    public void ShowPrevPanel()
    {
        ShowPanel(prevPanel);
    }

    public void ShowPanel(Panel panel)
    {
        switch (panel)
        {
            case Panel.None:
                break;
            case Panel.AllStocks:
                ShowAllStocks();
                break;
            case Panel.OwnedStocks:
                ShowOnedStocks();
                break;
            case Panel.StockPanel:
                if(lastStock != null)
                    ShowStock(lastStock);
                break;
            default:
                break;
        }
    }


    public void ShowAllStocks()
    {
        HideAllPanels();

        prevPanel = currentPanel;
        currentPanel = Panel.AllStocks;

        allStockList.SetActive(true);
    }   
    
    public void ShowOnedStocks()
    {
        HideAllPanels();

        prevPanel = currentPanel;
        currentPanel = Panel.OwnedStocks;

        ownedStockList.SetActive(true);
    }

    public void ShowStock(Stock stock)
    {
        HideAllPanels();

        lastStock = stock;

        prevPanel = currentPanel;
        currentPanel = Panel.StockPanel;

        stockViewer.gameObject.SetActive(true);

        stockViewer.Show(stock);
    }
}
