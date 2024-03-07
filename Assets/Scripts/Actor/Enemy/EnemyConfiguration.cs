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

public class EnemyConfiguration : ScriptableObject {

    public EnemyId Id;
    public Enemy Prefab;
    public float Health;
    public float Damage;
    [SerializeReference] public EnemyBehaviour Behaviour;
}
