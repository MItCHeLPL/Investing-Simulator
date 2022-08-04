using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using NaughtyAttributes;

public class ThemeController : MonoBehaviour
{
    //Singleton
    public static ThemeController Instance { get; private set; } 

    [Header("Themes")]
    public List<Theme> Themes;

    [Header("States")]
    [ReadOnly]
    public Theme ActiveTheme;

    [Header("Settings")]
    private const string _activeThemeIdPrefKey = "ActiveThemeID";

    [Header("Events")]
    public UnityEvent OnThemeChange;


    //Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
	{
        if (Themes.Count > 0)
        {
            int activeThemeId = 0;

            if (PlayerPrefs.HasKey(_activeThemeIdPrefKey))
            {
                activeThemeId = PlayerPrefs.GetInt(_activeThemeIdPrefKey);
            }

            EnableTheme(Themes[activeThemeId]);
        }
    }


    public void EnableTheme(Theme theme)
	{
        ActiveTheme = theme;

        if(!Themes.Contains(ActiveTheme))
		{
            Themes.Add(theme);
        }

        PlayerPrefs.SetInt(_activeThemeIdPrefKey, Themes.IndexOf(ActiveTheme));

        OnThemeChange.Invoke();
    }


    //Changing theme from inspector
    [Space(15)]
    [Header("Changing Theme")]
    [SerializeField] private byte themeIdToEnable = 0;

    [Button]
    public void EnableTheme()
    {
        if(Themes.Count > 0 && themeIdToEnable < Themes.Count)
		{
            EnableTheme(Themes[themeIdToEnable]);
        }
    }
}