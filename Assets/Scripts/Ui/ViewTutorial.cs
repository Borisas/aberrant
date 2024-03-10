using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTutorial : UiView {
    private const string Key = "tutorial_seen";
    
    public static bool ShouldOpen() {
        return PlayerPrefs.GetInt(Key, 0) == 0;
    }

    private void OnEnable() {
        Time.timeScale = 0.0f;
    }

    private void OnDisable() {
        Time.timeScale = 1.0f;
    }

    public void OnOk() {
        gameObject.SetActive(false);
        PlayerPrefs.SetInt(Key, 1);
    }
}
