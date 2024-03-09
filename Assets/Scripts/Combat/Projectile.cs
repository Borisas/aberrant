using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : DamageEntity, ISetupHitterWithDirection {
    private Rigidbody2D _body = null;
    private TrailRenderer _trail = null;

    [SerializeField] private GameObject _destroyAnimation = null;
    [SerializeField] private float _speed;
    [SerializeField] private bool _rotateToDirection = true;
    private float _damage;
    private Vector2 _direction;
    bool _firstFrame = false;

    private static int counter = 0;
    public int index = 0;

    private void Awake() {
        _body = GetComponent<Rigidbody2D>();
        _trail = GetComponent<TrailRenderer>();
        index = counter++;
    }

    public void Setup(Actor owner, float damage, Vector2 direction) {

        // //Safety for if it spawns outside of bounds?
        // transform.position = Vector3.zero;
        
        _owner = owner;
        _damage = damage;
        _direction = direction;
        _firstFrame = true;

        if (_trail != null) {
            _trail.enabled = false;
        }

        if (_rotateToDirection) {
            var r = _direction.AngleDeg();
            var rot = transform.localRotation.eulerAngles;
            rot.z = r;
            transform.localRotation = Quaternion.Euler(rot);
        }

    }

    void Update() {
        if (_firstFrame) {
            if (_trail != null) {
                _trail.enabled = true;
                _trail.Clear();
            }
            _firstFrame = false;
        }

        var p = transform.position;
        if (Mathf.Abs(p.x) > 20.0f || Mathf.Abs(p.y) > 20.0f) {
            DestroySelf(false);
        }
    }

    private void FixedUpdate() {

        var p = _body.position + _direction * (_speed * Time.fixedDeltaTime);
        _body.MovePosition(p);
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

        if (_owner == null) {
            if (actor is Player) return;// cnat hit player
        }
        else {
            if (!_owner.CanHit(actor)) {
                return;
            }
        }

        actor.Hit(GenerateHit());
        DestroySelf();
    }

    void DestroySelf(bool anim = true) {
        
        gameObject.SetActive(false);

        if (anim) {
            if (_destroyAnimation != null) {
                var go = ObjectPool.Get(_destroyAnimation);
                var t = transform;
                go.transform.position = t.position;
                go.transform.rotation = t.rotation;
            }
        }
    }
}
