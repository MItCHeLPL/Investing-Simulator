using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class StockTransactionButtons : MonoBehaviour
{
    [SerializeField] private TransactionsController transactionsController;

    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;

    [SerializeField] private Stepper amountStepper;
    [SerializeField] private TextMeshProUGUI amountText;


    private void OnEnable()
    {
        buyButton.onClick.AddListener(BuyCurrentStockShares);
        sellButton.onClick.AddListener(SellCurrentStockShares);

        amountStepper.onValueChanged.AddListener(SetAmountText);
    }

    private void OnDisable()
    {
        buyButton.onClick.RemoveListener(BuyCurrentStockShares);
        sellButton.onClick.RemoveListener(SellCurrentStockShares);

        amountStepper.onValueChanged.RemoveListener(SetAmountText);
    }


    private void SetAmountText(int val)
    {
        amountText.SetText(val.ToString());
    }


    public void BuyCurrentStockShares()
    {
        transactionsController.BuyShares(StockViewer.CurrentStock.Symbol, StockViewer.CurrentStock.Values[^1], amountStepper.value);
    }

    public void SellCurrentStockShares()
    {
        transactionsController.SellShares(StockViewer.CurrentStock.Symbol, StockViewer.CurrentStock.Values[^1], amountStepper.value);
    }
}
