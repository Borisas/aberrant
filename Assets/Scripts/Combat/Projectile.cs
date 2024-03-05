using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : DamageEntity {
    private Rigidbody2D _body = null;
    
    [SerializeField] private float _speed;
    private float _damage;
    private Vector2 _direction;

    private void Awake() {
        _body = GetComponent<Rigidbody2D>();
    }

    public void Setup(Actor owner, float damage, Vector2 direction) {
        _owner = owner;
        _damage = damage;
        _direction = direction;
    }

    private void FixedUpdate() {
        
        var p = _body.position + _direction * (_speed * Time.fixedDeltaTime);
        _body.MovePosition(p);

        if (Mathf.Abs(p.x) > 20.0f || Mathf.Abs(p.y) > 20.0f) {
            DestroySelf();
        }
    }

    protected override HitInfo GenerateHit() {
        return new HitInfo {
            Owner = _owner,
            Damage = _damage
        };
    }

    private void OnTriggerEnter2D(Collider2D other) {

        var rb = other.attachedRigidbody;
        if (rb == null) return;
        var actor = rb.GetComponent<Actor>();
        if (actor == null) return;

        if (!_owner.CanHit(actor)) {
            return;
        }
        
        actor.Hit(GenerateHit());
        DestroySelf();
    }

    void DestroySelf() {
        gameObject.SetActive(false);
    }
}
