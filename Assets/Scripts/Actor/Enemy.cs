using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor {
    
    private float _attackTimer = 0.0f;
    private float _attackInterval = 0.45f;
    private float _range = 0.525f;
    
    public void Setup(EnemyConfiguration cfg) {

        SetTarget(Scene.Player);//force target player
        SetupHealth(cfg.Health);
    }
    

    protected override void LateUpdate() {
        base.LateUpdate();
        if (_attackTimer < _attackInterval) {
            _attackTimer += Time.deltaTime;
        }
    }

    protected override void FixedUpdate() {
        var t = GetTarget();
        if (t != null) {
            if (!GoToTarget(1.0f, _range * 0.9f)) {
                if (_attackTimer >= _attackInterval) {
                    t.Hit(new HitInfo{
                        Owner =  this,
                        Damage = 1.0f
                    });
                    _attackTimer -= _attackInterval;
                }
            }
        }
    }
    
}
