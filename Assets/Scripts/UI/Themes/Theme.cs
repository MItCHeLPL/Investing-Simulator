using UnityEngine;
using System;

[Serializable]
public struct Theme
{
    public string Name;
    public Color PrimaryBackgroundColor;
    public Color SecondaryBackgroundColor;
    public Color TextColor;
    public Color AccentColor1;
    public Color AccentColor2;

    public enum UseColor
	{
        Primary,
        Secondary,
        Accent1,
        Accent2
    }
}
