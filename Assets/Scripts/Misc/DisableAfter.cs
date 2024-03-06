using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfter : MonoBehaviour {

    [SerializeField] private float _duration = 3.0f;
    [SerializeField] private AppearAnimator _appear = null;
    private float _timer = 0.0f;

    private void OnEnable() {
        _timer = 0.0f;
    }

    private void Update() {
        _timer += Time.deltaTime;
        if (_timer >= _duration) {
            DestroySelf();
        }
    }

    void DestroySelf() {
        if (_appear != null) {
            _appear.PlayDisappear(() => { gameObject.SetActive(false); });
        }
        else {
            gameObject.SetActive(false);
        }

    }
}
