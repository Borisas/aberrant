using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Actor {
    
    [SerializeField] private SpriteRenderer _visual = null;
    [SerializeReference] [ReadOnly] private EnemyBehaviour _behaviour = null;

    protected override void Awake() {
        base.Awake();
        _hitAnim  = new HitAnimation("_BlinkRatio", 0.2f, _visual, _visualTransform);
    }

    public void Setup(EnemyConfiguration cfg) {
        _alive = true;
        SetTarget(Scene.Player);//force target player
        SetupHealth(cfg.Health);

        if (cfg.Behaviour != null) {
            _behaviour = NTUtils.DeepClone(cfg.Behaviour);
            _behaviour.Setup(this);
        }
    }

    protected override void Update() {
        base.Update();
        _behaviour.Update();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        _behaviour.FixedUpdate();
    }

    public override void Hit(HitInfo hit) {
        base.Hit(hit);
        Scene.WorldUiController.GetDamageNumbers().ShowNumbersHit(transform.position, hit);
    }

    protected override void Die() {
        _hitAnim.Kill();
        gameObject.SetActive(false);
        Scene.Effects.SpawnBloodDrop(transform, Random.Range(1,4));
    }
}
