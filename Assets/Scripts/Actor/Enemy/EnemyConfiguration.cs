using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyId {
    None = 0,
    Blob = 1,
    Eye = 2,
    Flower = 3,
    Hunchback = 4,
}

public struct EnemyInfoMin { 
    public EnemyId Id;
    public int Weight;
}

public class EnemyConfiguration : ScriptableObject {

    public EnemyId Id;
    public Enemy Prefab;
    public float Health;
    public float Damage;
    public int Weight;
    public float BloodDropMultiplier;
    [SerializeReference] public EnemyBehaviour Behaviour;
    

    public EnemyInfoMin GetInfoMin() {
        return new EnemyInfoMin {
            Id = Id,
            Weight = Weight
        };
    }
}
