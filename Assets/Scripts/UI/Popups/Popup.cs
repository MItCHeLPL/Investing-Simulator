using TMPro;
using UnityEngine;

public abstract class Popup : MonoBehaviour
{
    [SerializeField] protected GameObject popupWindow;
    [SerializeField] protected GameObject popupBackground;
    [SerializeField] protected TextMeshProUGUI text;


    protected virtual void Start()
    {
        HidePopup();
    }


    protected void ShowPopup(string content)
    {
        text.SetText(content);

        popupWindow.SetActive(true);
        popupBackground.SetActive(true);
    }

    protected void HidePopup()
    {
        popupWindow.SetActive(false);
        popupBackground.SetActive(false);
    }
}
