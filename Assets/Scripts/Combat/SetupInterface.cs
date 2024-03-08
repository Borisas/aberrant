
using UnityEngine;

public interface ISetupHitter {
    public void Setup(Actor o, float dmg);
}

public interface ISetupHitterWithDirection {
    public void Setup(Actor e, float dmg, Vector2 direction);
}