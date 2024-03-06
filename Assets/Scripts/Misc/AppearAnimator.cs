using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearAnimator : MonoBehaviour {
    private Animation _anim = null;
    private System.Action _onComplete;

    private void Awake() {
        _anim = GetComponent<Animation>();
    }

    void OnEnable() {

        PlayAppear();
    }


    public void PlayAppear() {
        _anim.Play("Appear");
    }

    public void PlayDisappear(System.Action complete) {
        _onComplete = complete;
        _anim.Play("Disappear");
    }

    public void OnDissapearFinished() {
        _onComplete?.Invoke();
        _onComplete = null;
    }

}
