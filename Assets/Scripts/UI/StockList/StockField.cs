using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockField : MonoBehaviour
{
    public TextMeshProUGUI Label;
    public TextMeshProUGUI Value;

    [HideInInspector] public WindowManager WindowManager;
    [HideInInspector] public Stock Stock;

    private Button button;


    private void Awake()
    {
        button = GetComponent<Button>();   
    }

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
