using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private RunInstance _instance = new RunInstance();
    GameConfig _config;
    [SerializeField] private WaveController _waveController = null;
    MutationController _mutationController = null;
    
    private void Awake() {
        
        _instance = new RunInstance();
        _mutationController = new MutationController();
        _config = Database.GetInstance().Main.GameConfig;
    }

    void Start() {
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

    public bool IsWaveInProgress() {
        return _waveController.IsInProgress();
    }

    public BloodAmount GetPriceRecovery() {
        return new BloodAmount(_config.PriceRecovery);
    }

    public BloodAmount GetPriceMutate() {
        return new BloodAmount(_config.PriceMutate);
    }

    public (float min, float max) GetBloodDropAmountBase() {
        return (
            min: _config.MinBloodDropBase,
            max: _config.MaxBloodDropBase
        );
    }

    public MutationController GetMutationController() {
        return _mutationController;
    }
}
