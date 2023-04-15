using System.IO;
using UnityEngine;

public class SystemIOJSONSerializer
{
    public static T Load<T>(string filename)
    {
        string path = PathForFilename(filename);

        if (FileExists(path))
        {
            return JsonUtility.FromJson<T>(File.ReadAllText(path));
        }

        return default;
    }

    public static void Save<T>(string filename, T data)
    {
        string path = PathForFilename(filename);

        File.WriteAllText(path, JsonUtility.ToJson(data));
    }

    public static bool FileExists(string filename)
    {
        return File.Exists(filename);
    }

    public static string PathForFilename(string filename)
    {
        return $"{Application.persistentDataPath}/{filename}";
    }
}
