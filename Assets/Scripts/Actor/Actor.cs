using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    public System.Action<Actor> OnDie;

    [SerializeField] private SpriteRenderer _visual = null;
    private Actor _target;
    protected Rigidbody2D _body = null;
    protected AgentNav2d _nav = null;
    protected bool _alive = true;
    protected float _health = 10.0f;
    protected float _maxHealth = 10.0f;

    protected virtual void Awake() {
        _body = GetComponent<Rigidbody2D>();
        _nav = new AgentNav2d();
    }

    protected void SetupHealth(float hp, float prc = 1.0f) {
        _maxHealth = hp;
        _health = Mathf.Min(hp * prc, _maxHealth);
    }

    protected bool GoTo(Vector2 position, float speed, float minDist = 0.4f) {
        
        var cp = _body.position;

        float sqrD = (cp - position).sqrMagnitude;
        if (sqrD < Mathf.Pow(minDist, 2.0f)) {
            return false;//no need to move anywhere
        }

        var np = _nav.GetNextPoint(cp,position);
        Vector2 dirVec = (np - cp).normalized;
        var moveTo = cp + (dirVec) * (Time.fixedDeltaTime * speed);
        
        _body.MovePosition(moveTo);

        return true;

    }

    protected bool GoToTarget(float speed, float minDist) {
        if (_target != null && _target._alive) {
            return GoTo(_target.transform.position, speed, minDist);
        }

        return false;
    }

    protected virtual void LateUpdate() {
        _visual.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100.0f);
    }

    protected virtual void FixedUpdate() { }

    protected Actor GetTarget() => _target;

    protected void SetTarget(Actor t) {
        if (_target != null) {
            _target.OnDie -= OnTargetDied;
        }

        _target = t;
        if (_target != null) {
            _target.OnDie += OnTargetDied;
        }
    }

    protected virtual void OnTargetDied(Actor prvTarget) {
    }

    public void Hit(HitInfo hit) {
        if (_alive == false) return;
        _health -= hit.Damage;
        if (_health <= 0.0f) {
            Die();
            OnDie?.Invoke(this);
        }
    }


    protected virtual void Die() {
        Destroy(gameObject);
    }

    public bool IsAlive() => _alive;
}
