using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemeElement : MonoBehaviour
{
	[SerializeField] private Theme.UseColor _useColor;


	private void Start()
	{
		ThemeController.Instance.OnThemeChange.AddListener(UpdateElementActiveTheme);
	}
	private void OnEnable()
	{
		if(ThemeController.Instance != null)
		{
			ThemeController.Instance.OnThemeChange.AddListener(UpdateElementActiveTheme);
		}
	}
	private void OnDisable()
	{
		ThemeController.Instance.OnThemeChange.AddListener(UpdateElementActiveTheme);
	}


	public void UpdateElementActiveTheme()
	{
		UpdateElementTheme(ThemeController.Instance.ActiveTheme);
	}
	public void UpdateElementTheme(Theme theme)
	{
		if (TryGetComponent(out TextMeshProUGUI textMeshProUGUI))
		{
			textMeshProUGUI.color = theme.TextColor;
		}
		else if (TryGetComponent(out MaskableGraphic maskableGraphic))
		{
			SetElementColor(maskableGraphic, theme);
		}
		else if (TryGetComponent(out Graphic graphic))
		{
			SetElementColor(graphic, theme);
		}
	}

	private void SetElementColor(dynamic element, Theme theme)
	{
		try
		{
			switch (_useColor)
			{
				case Theme.UseColor.Primary:
					{
						element.color = theme.PrimaryBackgroundColor;
						break;
					}
				case Theme.UseColor.Secondary:
					{
						element.color = theme.SecondaryBackgroundColor;
						break;
					}
				case Theme.UseColor.Accent1:
					{
						element.color = theme.AccentColor1;
						break;
					}
				case Theme.UseColor.Accent2:
					{
						element.color = theme.AccentColor2;
						break;
					}
				default:
					{
						element.color = theme.PrimaryBackgroundColor;
						break;
					}
			}
		}
		catch{ }
	}
}
