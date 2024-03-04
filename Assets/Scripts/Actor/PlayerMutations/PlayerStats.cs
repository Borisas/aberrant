using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MutationId {
    Invalid = 0,
    Spikes = 1,
    PoisonCloud = 2,
    Detector = 3,
    ExtraArm = 4,
    BloodHalo = 5,
}

public class MutationInstance {
    public MutationId Id;
    public int Level;
}

public class PlayerStats {

    public System.Action OnMutationChanged;

    List<MutationInstance> _activeMutations = new List<MutationInstance>();

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

    public void AddMutation(MutationId id) {
        bool found = false;
        for ( int i = 0; i < _activeMutations.Count; i++ ) {
            if ( _activeMutations[i].Id == id ) {
                var m = _activeMutations[i];
                m.Level++;
                _activeMutations[i] = m;
                found = true;
            }
        }

        if ( !found ) {
            _activeMutations.Add(new MutationInstance {
                Id = id,
                Level = 1
            });
        }

        OnMutationChanged?.Invoke();
    }

    public IEnumerable<MutationInstance> GetActiveMutations() => _activeMutations;

    public MutationInstance GetMutationInstance(MutationId id){ 
        for ( int i = 0; i < _activeMutations.Count; i++ ) {
            if ( _activeMutations[i].Id == id ) {
                return _activeMutations[i];
            }
        }
        return default;
    }

}

