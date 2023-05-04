using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class StockReseter : MonoBehaviour
{
    [SerializeField] private StockHolder stockHolder;
    [SerializeField] private WindowManager windowManager;


    public void AskForReset()
    {
        string content = $"Reset all moneny and owned stocks?";

        ConfirmPopup.OnAccept.AddListener(ResetStocks);
        ConfirmPopup.OnCancel.AddListener(Cancel);

        ConfirmPopup.Show(content);
    }

    private void ResetStocks()
    {
        stockHolder.ResetData();

        windowManager.ShowAllStocks();

        ConfirmPopup.OnAccept.RemoveListener(ResetStocks);
    }

    private void Cancel()
    {
        ConfirmPopup.OnAccept.RemoveListener(ResetStocks);
        ConfirmPopup.OnCancel.RemoveListener(Cancel);
    }
}
