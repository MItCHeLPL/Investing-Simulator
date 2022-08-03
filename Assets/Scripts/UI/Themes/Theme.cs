using UnityEngine;
using System;

[Serializable]
public struct Theme
{
    public string Name;
    public Color PrimaryBackgroundColor;
    public Color SecondaryBackgroundColor;
    public Color TextColor;
    public Color AccentColor;

    public enum UseColor
	{
        Primary,
        Secondary,
        Accent
	}
}
