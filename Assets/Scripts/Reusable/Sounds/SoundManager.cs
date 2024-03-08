using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

    [SerializeField] private SoundManagerData _data = null;
    private List<AudioSource> _sources = new List<AudioSource>();
    
    // private AudioSource _source = null;
    private static SoundManager _instance = null;
    
    void Awake() {
        _instance = this;
    }



    public static void Play(string sound) {
        if (_instance == null) return;

        var ef = _instance._data.GetSound(sound);
        _instance.PlaySound(ef);
    }


    void PlaySound(SoundEffect ef) {
        var src = GetIdleSource();
        src.clip = ef.Clip;
        src.volume = ef.Volume;
        src.Play();
    }


    AudioSource GetIdleSource() {

        foreach (var src in _sources) {
            if (src.isPlaying) continue;
            return src;
        }

        var n = gameObject.AddComponent<AudioSource>();
        _sources.Add(n);
        return n;
    }
}
