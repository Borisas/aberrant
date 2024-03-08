
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
        _labelWaveProgress.text = $"{Scene.WaveController.GetWaveTimeRemaining():0}s";
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

    public void OnContinueButton() {
        CloseIntermission();
        Scene.GameController.NextWave();
    }

    public void OnMutateButton() {
        
        if (!Scene.GameController.PurchaseMutate() ) return;
        
        _intermission.OnMutate();
        _intermission.Resetup();
    }

    public void OnRecoverButton() {
        
        if (!Scene.GameController.PurchaseRecover() ) return;

        _intermission.OnRecover();
        _intermission.Resetup();
    }

    public void OnMoreLifeButton() {
        
        if (!Scene.GameController.PurchaseMoreLife() ) return;
        
        _intermission.OnMoreLife();
        _intermission.Resetup();
    }
}