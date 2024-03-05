using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageEntity : MonoBehaviour {

    protected Actor _owner;
    protected abstract HitInfo GenerateHit();
}
