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
    
    private void Awake() {

        Player = Object.FindObjectOfType<Player>();
        UiDirector = Object.FindObjectOfType<SimpleUi.Director>();
        GameController = Object.FindObjectOfType<GameController>();
        WorldUiController = Object.FindObjectOfType<WorldUiController>();
        Effects = Object.FindObjectOfType<Effects>();
    }
}
