using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour {
    private Transform _flyTo = null;
    private float _speed = 1.5f;
    private const float _acceleration = 2.0f;
    private Transform _transform;

    private void Awake() {
        _flyTo = Scene.Player.transform;
        _transform = transform;
    }

    private void OnEnable() {
        _speed = 0.5f;
    }

    Vector3 GetTargetPos() {
        var p = _flyTo.position;
        p.y += 0.35f;
        return p;
    }

    private void Update() {
        var vec = (GetTargetPos() - _transform.position);

        if (vec.sqrMagnitude > Mathf.Pow(0.01f, 2.0f)) {
            var v = vec.normalized;
            var move = v * (Time.deltaTime * _speed);
            if (move.magnitude > vec.magnitude) {
                move = vec;
            }
            _transform.position += move;
            _speed += _acceleration * Time.deltaTime;
        }
        else {
            OnTargetReached();
        }
    }

    void OnTargetReached() {

        var ba = new BloodAmount(1);
        ba.Add();
        
        gameObject.SetActive(false);
    }
}
