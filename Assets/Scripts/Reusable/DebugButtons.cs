using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButtons : MonoBehaviour {
    private int _speedUpKey = -1;
    private int _slowDownKey = -1;
    
    public void Awake() {
        if (!Application.isEditor) {
            Destroy(this);
        }
    }

    private void Update() {

        if (Input.GetKeyUp(KeyCode.R)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }

        if (Input.GetKeyDown(KeyCode.F1)) {
            _slowDownKey = TimeControl.AddOverride(0.5f);
        }
        else if (Input.GetKeyUp(KeyCode.F1)) {
            TimeControl.RemoveOverride(_slowDownKey);
        }

        if (Input.GetKeyDown(KeyCode.F2)) {
            _speedUpKey = TimeControl.AddOverride(2.0f);
        }
        else if (Input.GetKeyUp(KeyCode.F2)) {
            TimeControl.RemoveOverride(_speedUpKey);
        }
    }
}
