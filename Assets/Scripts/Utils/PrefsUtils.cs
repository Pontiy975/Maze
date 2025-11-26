using Newtonsoft.Json;
using UnityEngine;

public static class PrefsUtils
{
    public static T Load<T>(T defaultData, out string data) where T : class
    {
        return Load(defaultData, out data, typeof(T).Name);
    }

    public static T Load<T>(T defaultData, out string data, string saveKey) where T : class
    {
        if (!PlayerPrefs.HasKey(saveKey))
        {
            Save(defaultData, out data, saveKey);
            return defaultData;
        }

        data = PlayerPrefs.GetString(saveKey);
        return JsonConvert.DeserializeObject<T>(data);
    }

    public static void Save<T>(T instance, out string data) where T : class
    {
        string saveKey = typeof(T).Name;
        Save(instance, out data, saveKey);
    }

    public static void Save<T>(T instance, out string data, string saveKey) where T : class
    {
        data = JsonConvert.SerializeObject(instance);
        PlayerPrefs.SetString(saveKey, data);
        PlayerPrefs.Save();
    }
}