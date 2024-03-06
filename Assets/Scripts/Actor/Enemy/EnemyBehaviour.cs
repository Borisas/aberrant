using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EnemyBehaviour {
    
    [System.NonSerialized] protected Enemy _owner = null;
    [System.NonSerialized] protected EnemyConfiguration _config = null;

    public virtual void Setup(Enemy e, EnemyConfiguration cfg) {
        _owner = e;
        _config = cfg;
    }
    public abstract void Update();
    public abstract void FixedUpdate();
}
