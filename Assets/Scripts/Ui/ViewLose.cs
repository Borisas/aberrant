using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewLose : UiView {
    [SerializeField] private TMP_Text _labelWaves = null;
    [SerializeField] private TMP_Text _labelBlood = null;
    [SerializeField] private TMP_Text _labelKills = null;

    public void OnReturnButton() {
        SceneManager.LoadScene("MenuScene");
    }
}
