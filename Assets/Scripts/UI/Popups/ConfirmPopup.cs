using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmPopup : Popup
{
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button cancelButton;

    public static UnityEvent OnAccept = new();
    public static UnityEvent OnCancel = new();

    public static UnityEvent<string> ShowEvent = new();
    public static UnityEvent HideEvent = new();

    public static void Show(string content) => ShowEvent.Invoke(content);
    public static void Hide() => HideEvent.Invoke();


    private void OnEnable()
    {
        ShowEvent.AddListener(ShowPopup);
        HideEvent.AddListener(HidePopup);

        acceptButton.onClick.AddListener(AcceptPopup);
        cancelButton.onClick.AddListener(CancelPopup);
    }

    private void OnDisable()
    {
        ShowEvent.RemoveListener(ShowPopup);
        HideEvent.RemoveListener(HidePopup);

        acceptButton.onClick.RemoveListener(AcceptPopup);
        cancelButton.onClick.RemoveListener(CancelPopup);
    }


    private void AcceptPopup()
    {
        OnAccept.Invoke();
        HidePopup();
    }

    private void CancelPopup()
    {
        OnCancel.Invoke();
        HidePopup();
    }
}
