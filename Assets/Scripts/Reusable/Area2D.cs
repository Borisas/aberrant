using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Area2D : MonoBehaviour {

    [SerializeField] private Vector2 _size;

    private Vector2 _lastPoint = Vector2.zero;

    public Vector2 GetRandomPoint() {

        var p = new Vector2(
            Random.Range(0.0f, _size.x) - _size.x / 2.0f,
            Random.Range(0.0f, _size.y) - _size.y / 2.0f
        );

        return transform.localToWorldMatrix.MultiplyPoint(p);
    }

    public Vector2 GetRandomPointWithMinDeviation(float minDev) {
        var p = GetRandomPoint();

        for (int i = 0; i < 1000; i++) {
            if ((p - _lastPoint).sqrMagnitude > Mathf.Pow(minDev, 2.0f)) {
                break;
            }

            p = GetRandomPoint();
        }

        _lastPoint = p;
        return p;
    }

    private void OnDrawGizmos() {
        var mat = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = new Color(0.0f, 0.65f, 0.65f, 0.5f);
        Gizmos.DrawCube(Vector3.zero, new Vector3(_size.x, _size.y, 1.0f));
        
        Gizmos.matrix = mat;
    }
}
