using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Actor {

    public enum Limb {
        Invalid = 0,
        Head = 1,
        Body = 2,
        Leg_Left = 3,
        Leg_Right = 4,
        Arm_Left = 5,
        Arm_Right = 6
    }

    [System.Serializable]
    public class PlayerLimb {
        public Limb Limb;
        public SpriteRenderer Sprite;
    }

    [SerializeField] List<PlayerLimb> _limbs = new List<PlayerLimb>();
    [FormerlySerializedAs("_loadedVisuals")] [SerializeField] [ReadOnly] private List<MutationId> _loadedMutations = new List<MutationId>();
    private PlayerStats _stats = new PlayerStats();
    private readonly Collider2D[] _possibleTargets = new Collider2D[32];

    private float _attackTimer = 0.0f;

    protected override void Awake() {
        base.Awake();
        SetupHealth(100.0f);
        
        _hitAnim  = new HitAnimation("_BlinkRatio", 0.2f, _limbs.Select(x=>x.Sprite).ToArray(), _visualTransform);
        _stats.OnMutationChanged += Stats_OnMutationsChanged;
    }

    protected override void LateUpdate() {
        base.LateUpdate();
        if (_attackTimer < _stats.GetAttackInterval()) {
            _attackTimer += Time.deltaTime;
        }
    }

    protected override void Update() {
        base.Update();
        _stats.Update();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        var t = GetTarget();
        if (t == null) {
            GetClosestTarget();
        }
        else {
            if (!GoToTarget(_stats.GetSpeed(), _stats.GetRange())) {
                if (_attackTimer >= _stats.GetAttackInterval()) {
                    AttackTarget();
                    Turn(t.transform.position.x > transform.position.x);
                    _attackTimer -= _stats.GetAttackInterval();
                }
            }
        }
    }

    void AttackTarget() {
        _stats.OnBeforeAttack();

        var hit = new HitInfo {
            Owner = this,
            Damage = _stats.GetDamage()
        };

        _stats.ModifyHitInfo(ref hit);

        var target = GetTarget();
        
        target.Hit(hit);
    }

    public override void OnKilled(in HitInfo hit, Actor k) {
        base.OnKilled(hit, k);
        _stats.OnKill(hit,k);
    }

    public void GoToPosition(Vector2 p) {
        GoTo(p, _stats.GetSpeed(), 0.05f);
    }

    void GetClosestTarget() {

        var size = Physics2D.OverlapCircleNonAlloc(transform.position, 10.0f, _possibleTargets, LayerMask.GetMask("Enemy"));
        if (size <= 0) return;

        float minD = float.MaxValue;
        Enemy e = null;
        for (int i = 0; i < size; i++) {

            var rb = _possibleTargets[i].attachedRigidbody;
            if (rb == null) continue;
            var enemy = rb.GetComponent<Enemy>();
            if (enemy == null) continue;
            
            
            if (enemy.IsAlive() == false) continue;
            float d = (enemy.transform.position - transform.position).sqrMagnitude;
            if (d < minD) {
                minD = d;
                e = enemy;
            }
        }

        if (e == null) return;

        SetTarget(e);
    }

    public override void Hit(HitInfo hit) {
        base.Hit(hit);

        if (GetTarget() != null) {
            var pme = transform.position;
            
            var d = (pme - hit.Owner.transform.position).sqrMagnitude;
            var dToTarget = (pme - GetTarget().transform.position).sqrMagnitude;
            if (d < dToTarget * 0.95f) {
                SetTarget(hit.Owner);
            }
        }
        else if ( hit.Owner != null ) {
            SetTarget(hit.Owner);
        }

        Scene.WorldUiController.GetDamageNumbers().ShowNumbersDamageTaken(transform.position, hit);
    }

    protected override void OnTargetDied(Actor prvTarget) {
        base.OnTargetDied(prvTarget);
        SetTarget(null);
        GetClosestTarget();//try find new target immediately.
    }

    public PlayerStats GetStats() => _stats;

    void Stats_OnMutationsChanged() {
        LoadVisuals();
    }

    void LoadVisuals() {
        var mutations = _stats.GetActiveMutations();

        List<MutationId> newMutationsLoaded = new List<MutationId>();

        foreach (var mutation in mutations) {
            if (_loadedMutations.Contains(mutation.Id)) continue;

            var cfg = Database.GetInstance().Main.GetMutationConfig(mutation.Id);

            var limb = _limbs.FirstOrDefault(x => x.Limb == cfg.Limb);
            if (limb == null) continue;
            
            if (cfg.Attach) {
                var go = Instantiate(Database.GetInstance().Main.MutationInstance, limb.Sprite.transform);
                go.sprite = cfg.Visual;
            }
            else {
                limb.Sprite.sprite = cfg.Visual;
            }

            newMutationsLoaded.Add(mutation.Id);
        }

        _loadedMutations.AddRange(newMutationsLoaded);
    }
}
