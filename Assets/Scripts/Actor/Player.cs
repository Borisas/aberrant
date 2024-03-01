using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor {
    private readonly Collider2D[] _possibleTargets = new Collider2D[32];

    private float _range = 0.525f;
    private float _attackTimer = 0.0f;
    private float _attackInterval = 0.2f;

    protected override void Awake() {
        base.Awake();
        SetupHealth(100.0f);
    }

    protected override void LateUpdate() {
        base.LateUpdate();
        if (_attackTimer < _attackInterval) {
            _attackTimer += Time.deltaTime;
        }
    }

    protected override void FixedUpdate() {
        base.LateUpdate();

        var t = GetTarget();
        if (t == null) {
            GetClosestTarget();
        }
        else {
            if (!GoToTarget(1.0f, _range * 0.9f)) {
                if (_attackTimer >= _attackInterval) {
                    t.Hit(new HitInfo{
                        Owner =  this,
                        Damage = 5.0f
                    });
                    _attackTimer -= _attackInterval;
                }
            }

        }

    }

    void GetClosestTarget() {

        var size = Physics2D.OverlapCircleNonAlloc(transform.position, 10.0f, _possibleTargets, LayerMask.GetMask("Enemy"));
        if (size <= 0) return;

        float minD = float.MaxValue;
        Enemy e = null;
        for (int i = 0; i < size; i++) {

            var rb = _possibleTargets[i].attachedRigidbody;
            if (rb == null) continue;
            var enemy = rb.GetComponent<Enemy>();
            if (enemy == null) continue;
            
            
            if (enemy.IsAlive() == false) continue;
            float d = (enemy.transform.position - transform.position).sqrMagnitude;
            if (d < minD) {
                minD = d;
                e = enemy;
            }
        }

        if (e == null) return;

        SetTarget(e);
    }

    public override void Hit(HitInfo hit) {
        base.Hit(hit);

        if (GetTarget() != null) {
            var pme = transform.position;
            
            var d = (pme - hit.Owner.transform.position).sqrMagnitude;
            var dToTarget = (pme - GetTarget().transform.position).sqrMagnitude;
            if (d < dToTarget * 0.95f) {
                SetTarget(hit.Owner);
            }
        }
        else if ( hit.Owner != null ) {
            SetTarget(hit.Owner);
        }

        Scene.WorldUiController.GetDamageNumbers().ShowNumbersDamageTaken(transform.position, hit);
    }

    protected override void OnTargetDied(Actor prvTarget) {
        base.OnTargetDied(prvTarget);
        SetTarget(null);
        GetClosestTarget();//try find new target immediately.
    }
}
