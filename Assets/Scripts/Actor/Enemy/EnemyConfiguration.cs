using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyId {
    None = 0,
    Blob = 1,
}

public class EnemyConfiguration : ScriptableObject {

    public EnemyId Id;
    public Enemy Prefab;
    public float Health;
    public float Damage;
    [SerializeReference] public EnemyBehaviour Behaviour;
}
