using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour {

    [SerializeField] private string _sound = "";

    private void OnEnable() {
        SoundManager.Play(_sound);
    }
}
