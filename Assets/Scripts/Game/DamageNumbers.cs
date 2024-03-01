using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageNumbers : MonoBehaviour {
    
    [SerializeField] private float _animDuration = 0.5f;
    [SerializeField] private float _animLift = 0.2f;
    
    private static readonly Color _colorCrit = new Color(1.0f, 1.0f, 0.0f,1.0f);
    private static readonly Color _colorDamage = new Color(1.0f, 1.0f, 1.0f,1.0f);
    private static readonly Color _colorLostHealth = new Color(1.0f, 0.0f, 0.0f,1.0f);
    private static readonly Vector2 _lift = new Vector2(0.0f, 0.5f);
    private static readonly Vector2 _randomness = new Vector2(0.1f, 0.1f);
    
    private List<TMP_Text> _numbers = new List<TMP_Text>();
    private Transform _transform = null;

    private void Awake() {
        _transform = transform;
    }

    private void Update() {

        for (int i = 0; i < _numbers.Count; i++) {
            var n = _numbers[i];
            float aStep = Time.deltaTime / _animDuration;
            float posStep = _animLift * (Time.deltaTime / _animDuration);

            var c = n.color;
            c.a -= aStep;

            if (c.a < 0.0f) {
                n.gameObject.SetActive(false);
                _numbers.RemoveAt(i);
                i--;
                break;
            }
            n.color = c;
            var p = n.transform.position;
            p.y += posStep;
            n.transform.position = p;
        }
    }


    private TMP_Text SpawnNumber(Vector2 at) {


        var n = ObjectPool.Get(Database.GetInstance().Main.DamageNumberLabel, _transform);

        n.color = _colorDamage;
        
        var nt = n.transform;

        nt.SetParent(_transform);
        
        var spawnP = at + _lift;
        spawnP.x += Random.Range(-_randomness.x, _randomness.x);
        spawnP.y += Random.Range(-_randomness.y, _randomness.y);
        
        nt.position = spawnP;
        
        nt.SetParent(_transform);

        _numbers.Add(n);

        return n;
    }

    public void ShowNumbersHit(Vector2 at, in HitInfo hit) {

        var t = SpawnNumber(at);
        t.color = _colorDamage;
        t.text = $"-{Mathf.FloorToInt(hit.Damage)}";
    }

    public void ShowNumbersDamageTaken(Vector2 at, in HitInfo hit) {
        var t = SpawnNumber(at);
        t.color = _colorLostHealth;
        t.text = $"-{Mathf.FloorToInt(hit.Damage)}";
    }
}
