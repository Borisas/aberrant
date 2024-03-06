using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public static class NTExtensions {


    public static float AngleRad(this Vector2 v) {
        return (float)Mathf.Atan2(v.y, v.x);
    }

    public static float AngleDeg(this Vector2 v) {
        float radians = v.AngleRad();
        return radians * (180f / (float)Mathf.PI);
    }

    public static T Random<T>(this List<T> self) {
        if (self.Count <= 0) return default;
        return self[UnityEngine.Random.Range(0, self.Count)];
    }

    public static T Random<T>(this T[] self) {
        if (self.Length <= 0) return default;
        return self[UnityEngine.Random.Range(0, self.Length)];
    }


    public static T ExtractRandom<T>(this List<T> self, bool keepOne = false) {
        if (self.Count <= 0) return default;
        var rIdx = UnityEngine.Random.Range(0, self.Count);
        var ret = self[rIdx];
        if (!keepOne || self.Count > 1) {
            self.RemoveAt(rIdx);
        }
        return ret;
    }
}