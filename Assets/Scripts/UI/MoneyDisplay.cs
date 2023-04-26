using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private StockHolder stockHolder;

    [SerializeField] private TextMeshProUGUI moneyText;


    private void OnEnable()
    {
        OwnedStocksHolder.OnOwnedMoneyChange.AddListener(UpdateMoneyDisplay);
    }

    private void OnDisable()
    {
        OwnedStocksHolder.OnOwnedMoneyChange.RemoveListener(UpdateMoneyDisplay);
    }


    private void UpdateMoneyDisplay()
    {
        moneyText.SetText($"${System.String.Format("{0:0.00}", stockHolder.OwnedStocksHolder.OwnedMoney)}");
    }
}
