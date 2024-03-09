using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewMainMenu : UiView {
    
    [SerializeField] TMP_Text _labelSound = null;


    private void OnEnable() {
        UpdateSoundButton();
    }


    void UpdateSoundButton() {
        _labelSound.text = (SoundManager.IsMute() ? "Sound Off" : "Sound On");
    }

    public void OnSoundButton() {
        SoundManager.ToggleMute();
        UpdateSoundButton();
    }

    public void OnBeginButton() {
        Scene.PlaySceneExit(() => {
            SceneManager.LoadScene("GameScene");
        });
    }

    public void OnExitButton() {
        Application.Quit(0);
    }
}
