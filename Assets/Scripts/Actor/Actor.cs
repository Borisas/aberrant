using UnityEngine;

public class Actor : MonoBehaviour {
    private const float MIN_X_DIFF_TO_TURN = 0.1f;

    public System.Action<Actor> OnDie;
    public System.Action<Actor> OnHealthChanged;

    [SerializeField] private bool _turnToMovement = false;
    [SerializeField] protected Transform _visualTransform = null;
    [SerializeField] private Collider2D _hitbox = null;
    [SerializeField] protected Transform _projectileSpawnPos;
    private Actor _target;
    private bool _acting = true;
    protected Rigidbody2D _body = null;
    protected AgentNav2d _nav = null;
    protected bool _alive = true;
    protected float _health = 10.0f;
    protected float _maxHealth = 10.0f;
    protected float _lastX = 0.0f;
    protected bool _canTurnAgain = true;
    protected HitAnimation _hitAnim;

    protected virtual void Awake() {
        _body = GetComponent<Rigidbody2D>();
        _nav = new AgentNav2d();
    }

    private void OnEnable() {
        _hitAnim.Kill();
    }

    protected virtual void SetupHealth(float hp, float prc = 1.0f) {
        _maxHealth = hp;
        _health = Mathf.Min(hp * prc, _maxHealth);
        OnHealthChanged?.Invoke(this);
    }

    public float GetHealth() => _health;
    public float GetMaxHealth() => _maxHealth;
    
    public void SetActing(bool a) => _acting = a;
    protected bool IsActing() => _acting;

    public void SetPhysicsEnabled(bool e) {
        if (e) {
            _body.isKinematic = false;
            var colliders = GetComponentsInChildren<Collider>(true);
            foreach (var c in colliders) {
                c.enabled = true;
            }
        }
        else {
            _body.isKinematic = true;
            var colliders = GetComponentsInChildren<Collider>(true);
            foreach (var c in colliders) {
                c.enabled = false;
            }
        }
    }

    public bool IsFullHealth() {
        return Mathf.Abs(_health - _maxHealth) < Mathf.Epsilon;
    }

    public bool GoTo(Vector2 position, float speed, float minDist = 0.4f) {

        var cp = _body.position;

        float sqrD = (cp - position).sqrMagnitude;
        if (sqrD < Mathf.Pow(minDist, 2.0f)) {
            return false;//no need to move anywhere
        }

        var np = _nav.GetNextPoint(cp, position);
        Vector2 dirVec = (np - cp).normalized;
        var moveTo = cp + (dirVec) * (Time.fixedDeltaTime * speed);

        _body.MovePosition(moveTo);

        return true;

    }

    public bool GoToTarget(float speed, float minDist) {
        if (_target != null && _target._alive) {
            return GoTo(_target.transform.position, speed, minDist);
        }
        return false;
    }

    protected virtual void Update() { }

    protected virtual void LateUpdate() {
        if (!IsActing()) return;
        if (_turnToMovement) {
            var x = transform.position.x;
            if (Mathf.Abs(_lastX - x) > MIN_X_DIFF_TO_TURN) {
                bool lastMoveRight = x > _lastX;
                Turn(lastMoveRight);
                _lastX = x;
            }
            else if (GetTarget() != null) {
                var t = GetTarget();
                var diff = t.transform.position - transform.position;
                bool lastMoveRight = diff.x > 0.0f;
                Turn(lastMoveRight);
            }
        }

        _canTurnAgain = true;
    }


    protected virtual void FixedUpdate() { }

    public void Turn(bool right) {
        if (!_canTurnAgain) return;
        _visualTransform.localRotation = Quaternion.Euler(new Vector3(0.0f, right ? 0.0f : 180.0f, 0.0f));
        _canTurnAgain = false;
    }

    public Actor GetTarget() => _target;

    protected void SetTarget(Actor t) {
        if (_target != null) {
            _target.OnDie -= OnTargetDied;
        }

        _target = t;
        if (_target != null) {
            _target.OnDie += OnTargetDied;
        }
    }

    protected virtual void OnTargetDied(Actor prvTarget) {
    }

    public virtual void Hit(HitInfo hit) {
        if (_alive == false) return;
        _health -= hit.Damage;
        OnHealthChanged?.Invoke(this);

        SoundManager.Play("hit");
        
        if (_health <= 0.0f) {
            Die();
            if (hit.Owner != null) {
                hit.Owner.OnKilled(hit, this);
            }

            OnDie?.Invoke(this);
        }
        else {
            PlayHitAnimation();
        }
    }

    public virtual void Heal(float amount) {

        _health = Mathf.Min(_maxHealth, _health + amount);
        OnHealthChanged?.Invoke(this);
    }

    public virtual void RestoreHealth(float health) {
        _health += health;
        _health = Mathf.Clamp(_health, 0.0f, _maxHealth);
        OnHealthChanged?.Invoke(this);
    }

    protected void PlayHitAnimation() {
        _hitAnim.Play();
    }

    public void ForceDie() {
        Die();
    }

    protected virtual void Die() {
        _alive = false;
        _hitAnim.Kill();
        Destroy(gameObject);
    }

    public bool IsAlive() => _alive;

    public bool CanHit(Actor other) {
        return this.GetType() != other.GetType();
    }

    public virtual void OnKilled(in HitInfo hit, Actor k) { }

    public Collider2D GetHitbox() => _hitbox;
    public Transform GetProjectileSpawnPos() => _projectileSpawnPos;
}
