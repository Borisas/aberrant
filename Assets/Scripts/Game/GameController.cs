using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private RunInstance _instance = new RunInstance();
    [SerializeField] private WaveController _waveController = null;

    private void Awake() {
        
        _instance = new RunInstance();
    
        _waveController.BeginWave(_instance.WaveIndex);
        _waveController.OnWaveCompleted += WaveController_OnWaveCompleted;
    }

    void Update() {
        if (!_waveController.IsInProgress()) {
            Scene.Player.GoToPosition(Vector2.zero);
        }
    }

    private void WaveController_OnWaveCompleted() {
    }

    public RunInstance GetRunInstance() {
        return _instance;
    }
}
