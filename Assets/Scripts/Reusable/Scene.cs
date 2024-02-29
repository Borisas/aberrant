using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Scene : MonoBehaviour {
    
    public static SimpleUi.Director UiDirector;
    public static GameController GameController;
    public static Player Player;
    
    private void Awake() {

        Player = Object.FindObjectOfType<Player>();
        UiDirector = Object.FindObjectOfType<SimpleUi.Director>();
        GameController = Object.FindObjectOfType<GameController>();
    }
}
