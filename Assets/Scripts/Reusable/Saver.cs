using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Saver {

    public static void SaveJson<T>(string key, T obj) {
        PlayerPrefs.SetString(key,JsonWrapper.Serialize(obj));
    }

    public static T LoadJson<T>(string key) {
        return JsonWrapper.Deserialize<T>(PlayerPrefs.GetString(key));
    }

    public static bool KeyExists(string key) {
        return PlayerPrefs.HasKey(key);
    }
}
