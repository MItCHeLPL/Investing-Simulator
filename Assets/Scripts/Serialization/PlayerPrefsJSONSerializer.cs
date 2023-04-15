using UnityEngine;

public class PlayerPrefsJSONSerializer
{
    public static T Load<T>(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
        }

        return default;
    }

    public static void Save<T>(string key, T data)
    {
        PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
    }
}
