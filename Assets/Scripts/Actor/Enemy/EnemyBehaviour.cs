using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EnemyBehaviour {
    
    [System.NonSerialized] protected Enemy _owner = null;

    public virtual void Setup(Enemy e) {
        _owner = e;
    }
    public abstract void Update();
    public abstract void FixedUpdate();
}
