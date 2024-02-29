
using System;
using Newtonsoft.Json;

public static class JsonWrapper {
    private static JsonSerializerSettings _settings = null;
    
    public static string Serialize<T>(T data) {
        return JsonConvert.SerializeObject(data, GetSettings());
    }

    public static T Deserialize<T>(string json) {
        return JsonConvert.DeserializeObject<T>(json, GetSettings());
    }

    public static JsonSerializerSettings GenerateSettings() {
        return new JsonSerializerSettings();
    }

    static JsonSerializerSettings GetSettings() {
        if (_settings == null) {
            _settings = GenerateSettings();
        }
        return _settings;
    }
}