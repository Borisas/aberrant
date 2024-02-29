using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif


public static class NTUtils {
    
    public delegate void DelVoidVoid();
    
    public class Timer {
        private bool _running = false;
        private float _duration;
        private float _timer;
        private DelVoidVoid _onComplete;
        private bool _looping = false;

        public void Start(float duration) {
            _duration = duration;
            Restart();
            _running = true;
        }

        public void Restart() {
            _timer = 0.0f;
            _running = true;
        }

        public void Stop() {
            _running = false;
        }

        public void SetOnComplete(DelVoidVoid c) {
            _onComplete = c;
        }

        public void SetLooping(bool l) {
            _looping = l;
        }

        public void Complete() {
            _timer = _duration;
        }

        public void Run() {
            if (!_running) return;

            _timer += Time.deltaTime;

            if (_timer >= _duration) {
                _onComplete();
                if (!_looping) {
                    _running = false;
                }
                else {
                    _timer -= _duration;
                }
            }
        }

    }
    
    public static T DeepClone<T>(T obj) {
        UnityEngine.Profiling.Profiler.BeginSample("NTUtils.DeepClone()");
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;
            UnityEngine.Profiling.Profiler.EndSample();
            return (T) formatter.Deserialize(ms);
        }
    }

    public static void SetLayerRecursively(GameObject start, int layer) {
        start.layer = layer;
        var t = start.transform;

        for (int i = 0; i < t.childCount; i++) {
            SetLayerRecursively(t.GetChild(i).gameObject,layer);
        }
    }

    public static List<T> GetEnumList<T>() where T : struct, IConvertible {
        return new List<T>((T[]) Enum.GetValues(typeof(T)));
    }

#if UNITY_EDITOR
    public static IEnumerable<T> FindAssetsByType<T>() where T : Object {
        var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        foreach (var t in guids) {
            var assetPath = AssetDatabase.GUIDToAssetPath(t);
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null) {
                yield return asset;
            }
        }
    }
#endif
}
