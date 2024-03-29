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

        Scene.Player.OnDie += Player_OnDie;
        Scene.Player.GetStats().OnMutationChanged += Player_OnMutationsChanged;
    }

    private void Player_OnMutationsChanged(MutationInstance mi) {
        ViewMutationInfo.Open(mi);
    }

    private void Player_OnDie(Actor obj) {
        PrimeTween.Tween.Delay(1.5f, () => {
            ViewLose.Open(false);
        });
    }

    void Update() {
        if (!_waveController.IsInProgress()) {
            Scene.Player.GoToPosition(Vector2.zero);
        }
    }

    private void WaveController_OnWaveCompleted() {

        if (Scene.Player.IsAlive() == false) return;//no complete! >:(


        if (_instance.WaveIndex >= _config.FinalWave-1) {
            //game win
            ViewLose.Open(true);
        }
        else {
            PrimeTween.Tween.Delay(1.5f, () => {
                if (!ViewMutation.Open()) {
                    Scene.UiDirector.GetView<ViewGameplay>().OpenIntermission();
                }
            });
        }
    }

    public void NextWave() {
        _instance.NextWave();
        _waveController.BeginWave(_instance.WaveIndex);
    }

    public GameConfig GetConfig() => _config;

    public RunInstance GetRunInstance() {
        return _instance;
    }

    public bool IsWaveInProgress() {
        return _waveController.IsInProgress();
    }

    public MutationController GetMutationController() {
        return _mutationController;
    }

    public void OnEnemyKilled(Enemy e) {
        _instance.EnemyKilled(e);
    }

    #region PURCHASES
    public bool PurchaseMutate() {

        var pr = Scene.GameController.GetPriceMutate();
        if (!pr.CanPay()) return false;
        pr.Remove();

        if (!ViewMutation.Open()) return false;
        _instance.OnMutationPurchased();
        return true;
    }

    public bool PurchaseRecover() {

        var pr = Scene.GameController.GetPriceRecovery();
        if (!pr.CanPay()) return false;
        pr.Remove();

        Scene.Player.RestoreHealth(Scene.Player.GetMaxHealth() - Scene.Player.GetHealth());
        _instance.OnRecoveryPurchased();
        return true;
    }

    public bool PurchaseMoreLife() {

        var pr = Scene.GameController.GetPriceMoreLife();
        if (!pr.CanPay()) return false;
        pr.Remove();

        Scene.Player.IncreaseLife(Scene.GameController.GetConfig().MoreLifeIncrease);
        _instance.OnMoreLifePurchased();
        return true;
    }
    #endregion

    #region CONFIGS
    public BloodAmount GetPriceRecovery() {
        return new BloodAmount(_config.PriceRecovery
            + _instance.PurchasedRecoveries * _config.PriceRecoveryIncrease
        );
    }

    public BloodAmount GetPriceMutate() {
        return new BloodAmount(_config.PriceMutate
            + _instance.PurchasedMutations * _config.PriceMutateIncrease
        );
    }

    public BloodAmount GetPriceMoreLife() {
        return new BloodAmount(_config.PriceMoreLife
            + _instance.PurchasedMoreLife * _config.PriceMoreLifeIncrease
        );
    }

    public (float min, float max) GetBloodDropAmountBase() {
        return (
            min: _config.MinBloodDropBase,
            max: _config.MaxBloodDropBase
        );
    }

    public int GetTurretDamage() {
        return _config.TurretBaseDamage;
    }
    #endregion
}
