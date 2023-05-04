using System.Collections;
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

        StartCoroutine(RefreshStepper());
    }

    private void OnDisable()
    {
        buyButton.onClick.RemoveListener(BuyCurrentStockShares);
        sellButton.onClick.RemoveListener(SellCurrentStockShares);

        amountStepper.onValueChanged.RemoveListener(SetAmountText);

        StopAllCoroutines();
    }


    private void SetAmountText(int val)
    {
        amountText.SetText(val.ToString());
    }


    public void BuyCurrentStockShares()
    {
        transactionsController.BuyShares(StockViewer.CurrentStock.Symbol, StockViewer.CurrentStock.Values[0], amountStepper.value);

        StopAllCoroutines();
        StartCoroutine(RefreshStepper());
    }

    public void SellCurrentStockShares()
    {
        transactionsController.SellShares(StockViewer.CurrentStock.Symbol, StockViewer.CurrentStock.Values[0], amountStepper.value);

        StopAllCoroutines();
        StartCoroutine(RefreshStepper());
    }

    public IEnumerator RefreshStepper()
    {
        amountStepper.maximum = 99;

        amountStepper.value = 1;

        SetAmountText(1);

        yield return new WaitUntil(() => StockViewer.CurrentStock != null);

        amountStepper.maximum = GetMaxStepperValue();

        if(amountStepper.maximum < amountStepper.value)
        {
            amountStepper.value = 0;

            SetAmountText(0);
        }

        amountStepper.StepDown(); //Refresh UI
    }


    private int GetMaxStepperValue()
    {
        int canBuyCount = Mathf.FloorToInt((float)(transactionsController.OwnedMoney / StockViewer.CurrentStock.CurrentValue));

        if (transactionsController.stockHolder.OwnedStocksHolder.TryGetOwnedStock(StockViewer.CurrentStock.Symbol, out OwnedStock ownedStock))
        {
            if (ownedStock.Shares.Count > canBuyCount)
            {
                return ownedStock.Shares.Count;
            }
            else
            {
                return canBuyCount;
            }
        }
        else
        {
            return canBuyCount;
        }
    }
}
