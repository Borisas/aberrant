using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingZone : DamageEntity {

    [SerializeField] private float _tickInterval = 0.25f;
    [SerializeField] private float _duration = 5.0f;

    private List<Actor> _actors = new List<Actor>();
    
    private float _damage = 0.0f;
    private float _timer = 0.0f;
    private float _hitTimer = 0.0f;
    
    public void Setup(Actor owner, float tickDamage) {
        _owner = owner;
        _damage = tickDamage;
        _hitTimer = 0.0f;
        _timer = 0.0f;
        _actors.Clear();
    }
    

    void Update() {
        float dt = Time.deltaTime;
        _timer += dt;
        _hitTimer += dt;

        if (_hitTimer >= _tickInterval) {
            HitAll();
            _hitTimer -= _tickInterval;
        }

        if (_timer >= _duration) {
            DestroySelf();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var rb = other.attachedRigidbody;
        if (rb == null) return;
        var actor = rb.GetComponent<Actor>();
        if (actor == null) return;

        if (!_owner.CanHit(actor)) return;
        
        _actors.Add(actor);
    }

    private void OnTriggerExit2D(Collider2D other) {
        
        var rb = other.attachedRigidbody;
        if (rb == null) return;
        var actor = rb.GetComponent<Actor>();
        if (actor == null) return;
        _actors.Remove(actor);
    }

    void HitAll() {
        for (int i = 0; i < _actors.Count; i++) {
            Hit(_actors[i]);
        }
    }

    void Hit(Actor a) {
        a.Hit(GenerateHit());
    }

    protected override HitInfo GenerateHit() {
        return new HitInfo {
            Owner = _owner,
            Damage = _damage
        };
    }

    void DestroySelf() {
        gameObject.SetActive(false);
    }
}