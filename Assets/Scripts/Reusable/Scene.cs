using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Scene : MonoBehaviour {

    public static SimpleUi.Director UiDirector;
    public static WorldUiController WorldUiController;
    public static GameController GameController;
    public static Player Player;
    public static Effects Effects;
    public static WaveController WaveController;
    public static Camera GameCamera;
    public static Animation SceneLoadOverlay;

    [SerializeField] Animation _sceneLoad = null;

    private void Awake() {

        Player = Object.FindObjectOfType<Player>();
        UiDirector = Object.FindObjectOfType<SimpleUi.Director>();
        GameController = Object.FindObjectOfType<GameController>();
        WaveController = Object.FindObjectOfType<WaveController>();
        WorldUiController = Object.FindObjectOfType<WorldUiController>();
        Effects = Object.FindObjectOfType<Effects>();
        GameCamera = Camera.main;
        SceneLoadOverlay = _sceneLoad;
    }

    public static void PlaySceneExit(System.Action onComplete) {
        if (SceneLoadOverlay == null) {
            onComplete?.Invoke();
        }
        else {
            PrimeTween.Tween.Delay(0.55f, onComplete);
            SceneLoadOverlay.gameObject.SetActive(true);
            SceneLoadOverlay.Stop();
            SceneLoadOverlay.Play("SceneLoad_Appear");
        }
    }
}
