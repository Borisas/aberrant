using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor {
    public void Setup(EnemyConfiguration cfg) {

        SetTarget(Scene.Player);//force target player
        SetupHealth(cfg.Health);
    }

    private void Update() {
        var t = GetTarget();
        if (t != null) {
            GoToTarget(3.5f, 0.4f);
        }
    }
}
