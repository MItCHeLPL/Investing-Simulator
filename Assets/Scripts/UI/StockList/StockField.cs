using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockField : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI FullName;
    public TextMeshProUGUI Amount;
    public TextMeshProUGUI Value;
    public TextMeshProUGUI Balance;

    [HideInInspector] public WindowManager WindowManager;
    [HideInInspector] public Stock Stock;

    public Button button;


    private void OnEnable()
    {
        button.onClick.AddListener(ShowStock);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ShowStock);
    }


    private void ShowStock()
    {
        WindowManager.ShowStock(Stock);
    }
}
