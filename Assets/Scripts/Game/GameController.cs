using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private RunInstance _instance = new RunInstance();
    [SerializeField] private WaveController _waveController = null;

    private void Awake() {
    
        _waveController.BeginWave();
        _waveController.OnWaveCompleted += WaveController_OnWaveCompleted;
        _instance = new RunInstance();
    }

    private void WaveController_OnWaveCompleted() {
        
    }

    public RunInstance GetRunInstance() {
        return _instance;
    }
}
