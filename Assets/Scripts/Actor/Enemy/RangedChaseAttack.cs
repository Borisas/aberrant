using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangedChaseAttack : EnemyBehaviour {
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _attackSpeed = 2.0f;
    [SerializeField] private float _range = 0.525f;
    [SerializeField] private Projectile _projectile = null;
    
    private float _attackTimer = 0.0f;

    float GetAttackInterval() {
        return 1.0f / _attackSpeed;
    }

    public override void Update() {
        
        if (_attackTimer < GetAttackInterval()) {
            _attackTimer += Time.deltaTime;
        }
    }

    public override void FixedUpdate() {
        
        var t = _owner.GetTarget();
        if (t != null) {
            if (!_owner.GoToTarget(_speed, _range * 0.9f)) {
                if (_attackTimer >= GetAttackInterval()) {

                    var targetPos = t.GetHitbox().transform.position;
                    var startPos = _owner.GetProjectileSpawnPos().position;
                    var dir = (targetPos - startPos).normalized;
                    
                    var proj = ObjectPool.Get(_projectile);
                    proj.Setup(_owner,_config.Damage, dir);
                    proj.transform.position = _owner.GetProjectileSpawnPos().position;
                    _owner.Turn(t.transform.position.x > _owner.transform.position.x);
                    _attackTimer -= GetAttackInterval();
                }
            }
        }
    }
}
