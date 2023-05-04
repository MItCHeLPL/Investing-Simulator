using UnityEngine.Events;

public class InfoPopup : Popup
{
    public static UnityEvent<string> ShowEvent = new();
    public static UnityEvent HideEvent = new();

    public static void Show(string content) => ShowEvent.Invoke(content);
    public static void Hide() => HideEvent.Invoke();


    private void OnEnable()
    {
        ShowEvent.AddListener(ShowPopup);
        HideEvent.AddListener(HidePopup);
    }

    private void OnDisable()
    {
        ShowEvent.RemoveListener(ShowPopup);
        HideEvent.RemoveListener(HidePopup);
    }
}
