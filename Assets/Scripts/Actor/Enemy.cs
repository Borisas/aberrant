using UnityEngine;

public class Enemy : Actor {
    
    [SerializeField] private SpriteRenderer _visual = null;
    
    private float _attackTimer = 0.0f;
    private float _attackInterval = 0.45f;
    private float _range = 0.525f;

    protected override void Awake() {
        base.Awake();
        _hitAnim  = new HitAnimation("_BlinkRatio", 0.2f, _visual, _visualTransform);
    }

    public void Setup(EnemyConfiguration cfg) {
        _alive = true;
        SetTarget(Scene.Player);//force target player
        SetupHealth(cfg.Health);
    }
    

    protected override void LateUpdate() {
        base.LateUpdate();
        if (_attackTimer < _attackInterval) {
            _attackTimer += Time.deltaTime;
        }
    }

    protected override void FixedUpdate() {
        var t = GetTarget();
        if (t != null) {
            if (!GoToTarget(1.0f, _range * 0.9f, ref _lastMoveRight)) {
                if (_attackTimer >= _attackInterval) {
                    t.Hit(new HitInfo{
                        Owner =  this,
                        Damage = 1.0f
                    });
                    _attackTimer -= _attackInterval;
                }
            }
        }
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
