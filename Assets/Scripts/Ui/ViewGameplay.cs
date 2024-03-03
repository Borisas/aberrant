
using System;
using TMPro;
using UnityEngine;

public class ViewGameplay : UiView {

    [SerializeField] private TMP_Text _labelPlayerHealth = null;
    [SerializeField] private TMP_Text _labelWaveProgress = null;
    [SerializeField] private TMP_Text _labelBlood = null;
    [Header("Intermission")] 
    [SerializeField] private GameplayIntermission _intermission = null;

    private void Start() {

        var player = Scene.Player;
        player.OnHealthChanged += Player_OnHealthChanged;
        _labelPlayerHealth.text = $"{player.GetHealth():0}";
        CloseIntermission();
    }

    private void Update() {
        _labelWaveProgress.text = $"{Scene.WaveController.GetEnemySpawnsRemaining()}";
        _labelBlood.text = $"{Scene.GameController.GetRunInstance().Blood}";
    }

    private void Player_OnHealthChanged(Actor player) {
        _labelPlayerHealth.text = $"{player.GetHealth():0}";
    }

    void CloseIntermission() {
        _intermission.Close();
    }

    public void OpenIntermission() {
        _intermission.Open();
    }

    public void OnRecoverButton() {
        
        var pr = Scene.GameController.GetPriceRecovery();
        if (!pr.CanPay()) return;
        pr.Remove();
        
        Scene.Player.RestoreHealth(Scene.Player.GetMaxHealth() - Scene.Player.GetHealth());
        _intermission.Resetup();
    }

    public void OnContinueButton() {
        CloseIntermission();
        Scene.GameController.NextWave();
    }
}