using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Actor {
    private const float ELITE_HEALTH_MULTIPLIER = 1.75f;
    private const float ELITE_SIZE_MULTIPLIER = 1.5f;
    private const float ELITE_DAMAGE_MULTIPLIER = 1.75f;

    [SerializeField] private SpriteRenderer _visual = null;
    [SerializeField] private CircleCollider2D _collider = null;
    [SerializeReference][ReadOnly] private EnemyBehaviour _behaviour = null;
    private bool _elite = false;
    private float _damageMultiplier = 1.0f;
    private NTUtils.CachedValue<Vector3> _initialScale = new NTUtils.CachedValue<Vector3>();
    private NTUtils.CachedValue<float> _initialColliderRadius = new NTUtils.CachedValue<float>();
    EnemyConfiguration _config;

    protected override void Awake() {
        base.Awake();
        _hitAnim = new HitAnimation("_BlinkRatio", 0.2f, _visual, _visualTransform);
    }

    public void Setup(EnemyConfiguration cfg, bool elite) {
        _alive = true;

        _initialScale.UpdateValueIfDirty(_visual.transform.localScale);
        _initialColliderRadius.UpdateValueIfDirty(_collider.radius);

        _config = cfg;

        SetTarget(Scene.Player);//force target player
        SetupElite(elite);
        SetupHealth(cfg.Health);

        if (cfg.Behaviour != null) {
            _behaviour = cfg.Behaviour.ShallowClone();
            _behaviour.Setup(this, cfg);
        }
    }

    void SetupElite(bool e) {
        _elite = e;

        Vector3 newScale = _initialScale.GetValue() * (_elite ? ELITE_SIZE_MULTIPLIER : 1.0f);
        _visual.transform.localScale = newScale;
        _hitAnim.SetOriginalScale(newScale);

        _collider.radius = _initialColliderRadius.GetValue() * (_elite ? ELITE_SIZE_MULTIPLIER : 1.0f);

        _damageMultiplier = _elite ? ELITE_DAMAGE_MULTIPLIER : 1.0f;

        _visual.sharedMaterial = _elite
            ? Database.GetInstance().Main.MaterialCharacterElite
            : Database.GetInstance().Main.MaterialCharacterRegular;
    }

    protected override void SetupHealth(float hp, float prc = 1) {
        if (_elite) {
            hp *= ELITE_HEALTH_MULTIPLIER;
        }

        base.SetupHealth(hp, prc);

    }

    protected override void Update() {
        if (!IsActing()) return;
        base.Update();
        _behaviour.Update();
    }

    protected override void FixedUpdate() {
        if (!IsActing()) return;
        base.FixedUpdate();
        _behaviour.FixedUpdate();
    }

    public override void Hit(HitInfo hit) {
        base.Hit(hit);
        Scene.WorldUiController.GetDamageNumbers().ShowNumbersHit(transform.position, hit);
    }

    protected override void Die() {
        _alive = false;
        _hitAnim.Kill();
        gameObject.SetActive(false);

        var baseBlood = Scene.GameController.GetBloodDropAmountBase();

        int bloodSpawn = Mathf.RoundToInt(Random.Range(
                baseBlood.min * _config.BloodDropMultiplier,
                baseBlood.max * _config.BloodDropMultiplier
            ));


        Scene.Player.GetStats().ModifyBloodSpawn(ref bloodSpawn);

        Scene.Effects.SpawnBloodDrop(transform, bloodSpawn);

        var go = ObjectPool.Get(Database.GetInstance().Effects.BloodSplatterer);
        go.transform.position = transform.position;

        Scene.GameController.OnEnemyKilled(this);
    }

    public float GetDamageMultiplier() => _damageMultiplier;
}
