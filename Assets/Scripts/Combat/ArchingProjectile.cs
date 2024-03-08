using UnityEngine;

public class ArchingProjectile : DamageEntity, ISetupWithStartEndPosition {

    Rigidbody2D _body = null;
    TrailRenderer _trail = null;
    SpriteRenderer _visual = null;
    Transform _visualTransform = null;
    [SerializeField] float _speed = 4.0f;
    [SerializeField] float _arcLift = 0.25f;
    [SerializeField] GameObject _explosion = null;
    [SerializeField] float _radius = 0.25f;
    [SerializeField] float _rotationSpeed = 90.0f;
    float _damage;
    Vector2 _targetPos;
    Vector2 _startPos;
    Vector2 _liftedMidPoint;
    float _duration = 0.0f;
    float _timer = 0.0f;
    int _mask;
    bool _firstFrame = false;
    bool _exploded = false;
    PrimeTween.Tween _destroyTween;

    void Awake() {
        _body = GetComponent<Rigidbody2D>();
        _trail = GetComponent<TrailRenderer>();
        _visual = GetComponentInChildren<SpriteRenderer>();
        _visualTransform = _visual.transform;

        _mask = LayerMask.GetMask("Hitbox");
    }

    public void Setup(Actor owner, float dmg, Vector2 sPosition, Vector2 ePosition) {
        _owner = owner;
        _damage = dmg;
        _targetPos = ePosition;
        _startPos = sPosition;
        _exploded = false;

        float d = (_targetPos - _startPos).magnitude;
        _duration = d / _speed;
        _duration = Mathf.Max(0.1f, _duration);

        float arcLiftMul = Mathf.Lerp(0.5f, 3.0f, d / 4.0f);

        _liftedMidPoint = (_targetPos + _startPos) / 2.0f;
        _liftedMidPoint.y += _arcLift * arcLiftMul;
        _timer = 0.0f;

        _firstFrame = true;
        if (_trail != null) {
            _trail.enabled = false;
            _trail.Clear();
        }

        _visual.gameObject.SetActive(true);

        _destroyTween.Stop();
    }

    void Update() {
        if (_firstFrame) {
            if (_trail != null) {
                _trail.enabled = true;
                _trail.Clear();
            }
            _firstFrame = false;
        }

        if (Mathf.Abs(_rotationSpeed) > Mathf.Epsilon) {
            var rot = _visualTransform.localRotation.eulerAngles;
            rot.z += _rotationSpeed * Time.deltaTime;
            _visualTransform.localRotation = Quaternion.Euler(rot);
        }
    }

    void FixedUpdate() {

        if (_exploded) return;

        _timer += Time.fixedDeltaTime;

        float w = _timer / _duration;
        _body.MovePosition(GetPosAtTime(w));

        if (w >= 1.0f) {
            _exploded = true;
            Explode();
            DestroySelf();
        }

    }

    Vector2 GetPosAtTime(float t) {

        t = Mathf.Clamp01(t);

        Vector2 a = (1.0f - t) * _startPos + t * _liftedMidPoint;
        Vector2 b = (1.0f - t) * _liftedMidPoint + t * _targetPos;

        return (1.0f - t) * a + t * b;
    }

    void Explode() {
        var hits = Physics2D.OverlapCircleAll(_body.position, _radius, _mask);
        if (hits == null || hits.Length <= 0) return;

        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].attachedRigidbody == null) continue;
            var rb = hits[i].attachedRigidbody;
            var actor = rb.GetComponent<Actor>();
            if (actor == null) continue;

            if (!_owner.CanHit(actor)) continue;

            actor.Hit(GenerateHit());
        }
    }

    void DestroySelf() {


        if (_explosion != null) {
            var exp = ObjectPool.Get(_explosion);
            exp.transform.position = _body.position;
        }

        if (_trail != null) {
            _visual.gameObject.SetActive(false);
            _destroyTween = PrimeTween.Tween.Delay(_trail.time, () => {
                gameObject.SetActive(false);
            });
        }
        else {
            gameObject.SetActive(false);
        }
    }

    protected override HitInfo GenerateHit() {
        return new HitInfo {
            Owner = _owner,
            Damage = _damage
        };
    }
}