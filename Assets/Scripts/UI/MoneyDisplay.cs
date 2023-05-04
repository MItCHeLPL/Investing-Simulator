using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TransactionsController transactionsController;

    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private Button moneyPopupButton;


    private void OnEnable()
    {
        OwnedStocksHolder.OnOwnedMoneyChange.AddListener(UpdateMoneyDisplay);

        moneyPopupButton.onClick.AddListener(ShowMoneyPopup);
    }

    private void OnDisable()
    {
        OwnedStocksHolder.OnOwnedMoneyChange.RemoveListener(UpdateMoneyDisplay);

        moneyPopupButton.onClick.RemoveListener(ShowMoneyPopup);
    }


    private void UpdateMoneyDisplay()
    {
        moneyText.SetText($"${System.String.Format("{0:0.00}", transactionsController.OwnedMoney)}");
    }

    private void ShowMoneyPopup()
    {
        double balance = transactionsController.GetTotalBalance();


        string balanceChar = "<color=black>";
        if (balance > 0) balanceChar = "<color=#008800>+";
        else if (balance < 0) balanceChar = "<color=red>-";

        string content = $"Available money: ${System.String.Format("{0:0.00}", transactionsController.OwnedMoney)}\n" +
            $"Money in stocks: ${System.String.Format("{0:0.00}", transactionsController.GetWorthFromOwnedStocks())}\n" +
            $"Total money: ${System.String.Format("{0:0.00}", transactionsController.GetTotalWorth())}\n" +
            $"Total Profit / Loss: {balanceChar}${System.String.Format("{0:0.00}", Mathf.Abs((float)balance))}</color>";

        InfoPopup.Show(content);
    }
}
