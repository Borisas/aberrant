
using System;
using TMPro;
using UnityEngine;

public class ViewGameplay : UiView {

    [SerializeField] private TMP_Text _labelPlayerHealth = null;

    private void Start() {

        var player = Scene.Player;
        player.OnHealthChanged += Player_OnHealthChanged;
        _labelPlayerHealth.text = $"{player.GetHealth():0}";
    }

    private void Player_OnHealthChanged(Actor player) {
        _labelPlayerHealth.text = $"{player.GetHealth():0}";
    }
}