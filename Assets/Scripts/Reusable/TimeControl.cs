using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class TimeControl {

    struct TimeOverride {
        public float TimeMul;
        public int Key;
    }

    private static List<TimeOverride> _overrides;
    private static bool _initialised = false;

    static void Init() {
        if (_initialised) return;
        _initialised = true;
        
        _overrides = new List<TimeOverride>();
    }

    public static int AddOverride(float v) {
        Init();
        var ov = new TimeOverride();
        ov.TimeMul = v;
        ov.Key = GetRandomKey();

        _overrides.Add(ov);
        RecheckTimeScale();
        return ov.Key;
    }

    public static void RemoveOverride(int key) {
        bool removed = false;
        for (int i = 0; i < _overrides.Count; i++) {
            if (_overrides[i].Key == key) {
                _overrides.RemoveAt(i);
                removed = true;
                break;
            }
        }

        if (removed) {
            RecheckTimeScale();
        }
    }

    static void RecheckTimeScale() {
        if (_overrides.Count <= 0) {
            Time.timeScale = 1.0f;
        }
        else {
            //^1 = last
            Time.timeScale = _overrides[^1].TimeMul;
        }
    }

    static int GetRandomKey() {

        bool isKeyUnique(int key) {
            for (int i = 0; i < _overrides.Count; i++) {
                if (_overrides[i].Key == key) return false;
            }
            return true;
        }

        int v = Random.Range(Int32.MinValue, Int32.MaxValue);
        while (!isKeyUnique(v)) {
            v = Random.Range(Int32.MinValue, Int32.MaxValue);
        }

        return v;
    }

}
