
using System;
using TMPro;
using UnityEngine;

public class ViewGameplay : UiView {

    [SerializeField] private TMP_Text _labelPlayerHealth = null;
    [SerializeField] private TMP_Text _labelWaveProgress = null;
    [SerializeField] private TMP_Text _labelBlood = null;

    private void Start() {

        var player = Scene.Player;
        player.OnHealthChanged += Player_OnHealthChanged;
        _labelPlayerHealth.text = $"{player.GetHealth():0}";
    }

    private void Update() {
        _labelWaveProgress.text = $"{Scene.WaveController.GetEnemiesRemainingAlive()}";
        _labelBlood.text = $"{Scene.GameController.GetRunInstance().Blood}";
    }

    private void Player_OnHealthChanged(Actor player) {
        _labelPlayerHealth.text = $"{player.GetHealth():0}";
    }
}