using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SoundEffect {
    [FormerlySerializedAs("Effect")] public AudioClip Clip;
    [Range(0.0f, 1.0f)] public float Volume = 1.0f;
}