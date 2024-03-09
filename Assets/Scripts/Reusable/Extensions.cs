using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public static class NTExtensions {

    public static void DestroyAllChildren(this Transform t) {
        for (int i = 0; i < t.childCount; i++) {
            GameObject.Destroy(t.GetChild(i).gameObject);
        }
    }

    public static float AngleRad(this Vector2 v) {
        return (float)Mathf.Atan2(v.y, v.x);
    }

    public static float AngleDeg(this Vector2 v) {
        float radians = v.AngleRad();
        return radians * (180f / (float)Mathf.PI);
    }

    public static Vector2 Rotate(this Vector2 v, float degrees) {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);

        return v;
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