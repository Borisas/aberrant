using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class SoundManagerData : ScriptableObject {

    public AudioMixer MixerMusic;
    public SoundEffect MusicTrack;
    public AudioMixer MixerSounds;
    public SerializableDictionary<string, SoundEffect> SoundEffects = new SerializableDictionary<string, SoundEffect>();

    public SoundEffect GetSound(string sname) {
        bool found = SoundEffects.TryGetValue(sname, out var ret);
        if (!found) return null;
        return ret;
    }
}
