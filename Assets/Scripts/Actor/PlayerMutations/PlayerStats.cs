using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMutationId {
    Invalid = 0,  
}

public class PlayerStats {


    public float GetDamage() => 5.0f;
    public float GetSpeed() => 1.5f;
    public float GetRange() => 0.525f;
    public float GetAttackInterval() => 0.2f;
    
    public void OnBeforeAttack() {

    }

    public void Update() {

    }

    public void ModifyHitInfo(ref HitInfo hit) {

    }

}

