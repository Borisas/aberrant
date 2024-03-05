using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private NTUtils.Timer _spikeTimer = new NTUtils.Timer();
    
    public float GetDamage() {
        
        const float baseV = 5.0f;
        float ret = baseV;

        int lvlExtraArm = GetMutationLevel(MutationId.ExtraArm);
        if (lvlExtraArm > 0) {
            var mcfg = Database.GetInstance().Main.GetMutationConfig(MutationId.ExtraArm);
            ret += mcfg.Values[0] + mcfg.ScaleValues[0] * (lvlExtraArm-1);
        }

        return ret;
    }
    public float GetSpeed() => 1.5f;
    public float GetRange() => 0.525f;
    public float GetAttackInterval() => 0.2f;
    
    public void OnBeforeAttack() {

    }

    public void Update() {
        _spikeTimer.Run();
    }

    public void ModifyHitInfo(ref HitInfo hit) {

    }

    public void OnKill(in HitInfo hit, Actor target) {

        int poisonCloud = GetMutationLevel(MutationId.PoisonCloud);
        if (poisonCloud > 0) {

            var cfg = Database.GetInstance().Main.GetMutationConfig(MutationId.PoisonCloud);
            
            float spawnChance = cfg.Values[1] + cfg.ScaleValues[1] * (float)(poisonCloud-1);
            
            bool spawn = Random.value <= spawnChance;
            if (spawn) {
                float damage = cfg.Values[0] + cfg.ScaleValues[0] * (float) (poisonCloud - 1);
                var cloud = ObjectPool.Get(Database.GetInstance().Main.PoisonCloud);
                cloud.Setup(Scene.Player,damage);
                cloud.transform.position = target.transform.position;
            }
        }
    }

    public int GetMutationLevel(MutationId id) {
        for (int i = 0; i < _activeMutations.Count; i++) {
            if (_activeMutations[i].Id == id) {
                return _activeMutations[i].Level;
            }
        }
        return 0;
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
        MutationsChanged(id);
    }

    void MutationsChanged(MutationId id) {


        if (id == MutationId.Spikes) {

            var cfg = Database.GetInstance().Main.GetMutationConfig(id);
            var lvl = GetMutationLevel(id);

            float fireInterval = cfg.Values[1];

            if (lvl > 1) {
                fireInterval *= 1.0f / (1.0f + (cfg.ScaleValues[1] * (float) lvl));
            }

            _spikeTimer.Stop();
            _spikeTimer = new NTUtils.Timer()
                .SetLooping(true)
                .SetOnComplete(FireSpike)
                .Start(fireInterval);
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


#region MUTATION SPECIFIC

    void FireSpike() {

        var lvl = GetMutationLevel(MutationId.Spikes);
        var cfg = Database.GetInstance().Main.GetMutationConfig(MutationId.Spikes);

        float dmg = cfg.Values[0] + (float) (lvl - 1) * cfg.ScaleValues[0];

        var e = GetClosestEnemy();
        if (e == null) return;

        var startPos = Scene.Player.GetProjectileSpawnPos();
        var targetPos = e.transform.position;
        var dir = (targetPos - startPos).normalized;
        
        var p = ObjectPool.Get(Database.GetInstance().Main.SpikeProjectile);
        p.Setup(Scene.Player, dmg, dir);
        p.transform.position = startPos;
    }

#endregion

#region MISC

    Actor GetClosestEnemy() {
        var mpos = Scene.Player.transform.position;

        var enemies = Scene.WaveController.GetEnemySpawner().GetEnemies();

        Actor closestActor = null;
        float sqrD = 0.0f;

        foreach (var e in enemies) {
            if (e.IsAlive() == false) continue;
            if (closestActor == null) {
                closestActor = e;
                sqrD = (mpos - e.transform.position).sqrMagnitude;
            }
            else {
                var newDist = (mpos - e.transform.position).sqrMagnitude;
                if (newDist < sqrD) {
                    closestActor = e;
                    sqrD = newDist;
                }
            }
        }

        return closestActor;
    }

#endregion
}

