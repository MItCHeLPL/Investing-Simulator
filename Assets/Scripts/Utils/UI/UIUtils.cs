using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUtils
{
    private static Dictionary<TextMeshProUGUI, string> savedTextFields = new Dictionary<TextMeshProUGUI, string>();


    public static void ReplaceText(TextMeshProUGUI textField, string from, object to)
    {
        ReplaceText(textField, new string[] { from }, new object[] { to });
    }
    public static void ReplaceText(TextMeshProUGUI textField, string[] from, object[] to)
    {
        string text;

        if (savedTextFields.ContainsKey(textField))
        {
            text = savedTextFields[textField];
        }
        else
        {
            text = textField.text;
            savedTextFields.Add(textField, text);
        }

        for(int i=0; i<from.Length; i++)
		{
            text = text.Replace(from[i], to[i].ToString());
        }

        textField.text = text;
    }
}
