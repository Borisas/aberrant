using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {
    private const string SoundKey = "SOUND";
    
    [SerializeField] private SoundManagerData _data = null;
    private List<AudioSource> _sources = new List<AudioSource>();
    private AudioSource _musicSource = null;
    private static SoundManager _instance = null;

    private bool _muteState = false;
    
    void Awake() {

        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        
        
        DontDestroyOnLoad(this);
    }

    void OnEnable() {
        _muteState = IsMute();
        PlayMusic();
    }

    public static void Play(string sound, float minInterval = 0.1f) {
        if (_instance == null) return;

        var ef = _instance._data.GetSound(sound);
        _instance.PlaySound(ef, minInterval);
    }

    public static void ToggleMute() {
        var v = PlayerPrefs.GetInt(SoundKey, 1);
        PlayerPrefs.SetInt(SoundKey, v == 1 ? 0 : 1);
        
        if (_instance != null) {
            _instance.MuteChanged();
        }
    }

    public static bool IsMute() => PlayerPrefs.GetInt(SoundKey, 1) != 1;

    void MuteChanged() {

        _muteState = IsMute();

        if (IsMute()) {
            foreach (var src in _sources) {
                src.Stop();
            }

            if (_musicSource != null) {
                _musicSource.Stop();
            }
        }
        else {
            PlayMusic();
        }
    }

    void PlayMusic() {

        if (_muteState) return;
        if (_data.MusicTrack == null) return;

        if (_musicSource == null) {
            _musicSource = GetIdleSource();
        }

        _musicSource.Stop();
        
        var src = _musicSource;
        src.loop = true;
        src.clip = _data.MusicTrack.Clip;
        src.volume = _data.MusicTrack.Volume;
        src.outputAudioMixerGroup = _data.MixerMusic.FindMatchingGroups("")[0];
        src.Play();
    }

    void PlaySound(SoundEffect ef, float minInterval) {

        if (_muteState) return;

        if (minInterval > 0.0f) {
            foreach (var s in _sources) {
                if (s.clip != ef.Clip) continue;
                if (s.time < minInterval) continue;
                return;
            }
        }

        var src = GetIdleSource();
        src.clip = ef.Clip;
        src.volume = ef.Volume;
        src.loop = false;
        src.outputAudioMixerGroup = _instance._data.MixerSounds.FindMatchingGroups("")[0];
        src.Play();
    }


    AudioSource GetIdleSource() {

        foreach (var s in _sources) {
            if (s.isPlaying) continue;
            return s;
        }

        var n = gameObject.AddComponent<AudioSource>();
        _sources.Add(n);
        return n;
    }
}
