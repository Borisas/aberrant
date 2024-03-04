using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private RunInstance _instance = new RunInstance();
    [SerializeField] private WaveController _waveController = null;
    MutationController _mutationController = null;

    private void Awake() {
        
        _instance = new RunInstance();
        _mutationController = new MutationController();
    
        _waveController.BeginWave(_instance.WaveIndex);
        _waveController.OnWaveCompleted += WaveController_OnWaveCompleted;
    }

    void Update() {
        if (!_waveController.IsInProgress()) {
            Scene.Player.GoToPosition(Vector2.zero);
        }
    }

    private void WaveController_OnWaveCompleted() {
        Scene.UiDirector.GetView<ViewGameplay>().OpenIntermission();
        ViewMutation.Open();
    }

    public void NextWave() {
        _instance.NextWave();
        _waveController.BeginWave(_instance.WaveIndex);
    }

    public RunInstance GetRunInstance() {
        return _instance;
    }


    public BloodAmount GetPriceRecovery() {
        return new BloodAmount(50);
    }

    public BloodAmount GetPriceMutate() {
        return new BloodAmount(50);
    }

    public MutationController GetMutationController() {
        return _mutationController;
    }
}
